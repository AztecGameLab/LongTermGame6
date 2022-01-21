using System.Text;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;

namespace Game.Enemy
{
    // todo: generalize
    
    public class TestingEnemyBehaviourTree : MonoBehaviour
    {
        public enum IdleType
        {
            Patrol,
            Guard,
            Wander,
            Follow
        }

        [SerializeField] private float viewRange;
        [SerializeField] private LineOfSight target;
        [SerializeField] private BehaviorTree tree;
        [SerializeField] private EnemyDebugView debugView;
        [SerializeField] private Transform viewTransform;
        [SerializeField] private IdleType type;
        
        private StringBuilder _taskBuilder = new StringBuilder();
        private StringBuilder _debugBuilder = new StringBuilder();
        
        private void Awake()
        {
            tree = new BehaviorTreeBuilder(gameObject)
                .Selector("Enemy States")
                    .Selector("Alert")
                        .IsVisible(viewTransform, target, viewRange)
                        .Sequence("Eliminate Target")
                            .Do(() => TaskStatus.Success)
                        .End()
                    .End()
                    .Selector("Suspicious")
                        .Condition("Noticed Something Suspicious", () => false)
                        .Sequence("Investigate Suspicions")
                            .Do(() => TaskStatus.Success)
                        .End()
                    .End()
                    .Selector("Idle")
                        .Sequence($"{type.ToString()}")
                            .Do(() => TaskStatus.Success)
                        .End()
                    .End()
                .End()
                .Build();
        }

        private void Update()
        {
            tree.Tick();
            UpdateDebug();
        }

        private void UpdateDebug()
        {
            string enemyName = gameObject.name;
            string enemyTasks = GetTaskDescriptions();
            string debugData = GetDebugData();
            
            debugView.enemyName.UpdateText(enemyName);
            debugView.enemyTasks.UpdateText(enemyTasks);
            debugView.debugData.UpdateText(debugData);
        }

        private string GetTaskDescriptions()
        {
            _taskBuilder.Clear();
            
            foreach (var task in tree.ActiveTasks)
                _taskBuilder.Append($"\n{task.Name}");

            return _taskBuilder.ToString();
        }

        private string GetDebugData()
        {
            string result = _debugBuilder.ToString();
            _debugBuilder.Clear();

            return result;
        }
    }
}