using System;
using System.Collections.Generic;
using ConsoleUtility;
using FMOD;
using FMODUnity;
using JetBrains.Annotations;
using UnityEngine;
using Console = ConsoleUtility.Console;

namespace Commands
{
    [AutoRegisterConsoleCommand, UsedImplicitly]
    public class AudioCommand : IConsoleCommand
    {
        public string name => "audio";
        public string summary => "Interface with the audio systems in the game.";
        public string help => @"
    * audio: Display debugging information about the audio.";
        
        public IEnumerable<Console.Alias> aliases { get; }
        
        public void Execute(string[] args)
        {
            HandleDebug();
        }

        private static void HandleDebug()
        {
            Console.RegisterView<AudioView>();
        }

        private class AudioView : View
        {
            private DSP _mixerHead;
            
            // Note: This long and disgusting method is copied and pasted from RuntimeManager.DrawDebugOverlay
            // Although I have made a slight attempt to clean it up, I don't really understand a lot of what it does.
            
            public override string GetDebugViewString()
            {
                InitializeMixerHead();

                string cpuData = GetCPUData();
                string memoryData = GetMemoryData();
                string channelData = GetChannelData();
                string volumeData = GetVolumeData();
                
                return $@"
    {cpuData}
    {memoryData}
    {channelData}
    {volumeData}";
            }
            
            private void InitializeMixerHead()
            {
                if (!_mixerHead.hasHandle())
                {
                    RuntimeManager.CoreSystem.getMasterChannelGroup(out ChannelGroup master);
                    master.getDSP(0, out _mixerHead);
                    _mixerHead.setMeteringEnabled(false, true);
                }
            }

            private static string GetCPUData()
            {
                RuntimeManager.StudioSystem.getCPUUsage(out var cpuUsage);
                return $"CPU: dsp = {cpuUsage.dspusage:F1}%, studio = {cpuUsage.studiousage:F1}%";
            }

            private static string GetMemoryData()
            {
                Memory.GetStats(out int currentAlloc, out int maxAlloc);
                return $"MEMORY: cur = {currentAlloc >> 20}MB, max = {maxAlloc >> 20}MB";
            }

            private static string GetChannelData()
            {
                RuntimeManager.CoreSystem.getChannelsPlaying(out int channels, out int realChannels);
                return $"CHANNELS: real = {realChannels}, total = {channels}";
            }

            private string GetVolumeData()
            {
                _mixerHead.getMeteringInfo(IntPtr.Zero, out var outputMetering);
                
                float rms = 0;
                
                for (int i = 0; i < outputMetering.numchannels; i++)
                    rms += outputMetering.rmslevel[i] * outputMetering.rmslevel[i];

                rms = Mathf.Sqrt(rms / outputMetering.numchannels);

                float db = rms > 0 
                    ? 20.0f * Mathf.Log10(rms * Mathf.Sqrt(2.0f)) 
                    : -80.0f;
                
                if (db > 10.0f) 
                    db = 10.0f;

                return $"VOLUME: RMS = {db:f2}db";
            }
        }
    }
}