using System.Collections;
using System.Collections.Generic;
using Akassets.SmoothGridLayout;
using CustomUtilities;
using UnityEngine;


namespace FightingMinigameLogic
{
    public class SkillsCountVisualizer : MonoBehaviour
    {
        [SerializeField] private SmoothGridLayoutUI grid;
        [SerializeField] private float skillPointAnimationTime = 0.3f;
        [SerializeField] private ScaleController skillPointPrefab;

        private List<ScaleController> _skillPoints = new List<ScaleController>();

        public void InstantiateSkillPoint()
        {
            var skill = Instantiate(skillPointPrefab, grid.elementsTransform);
            skill.transform.SetAsFirstSibling();
            TimeUtility.CallWithDelay(.1f, () => skill.ChangeScale(new Vector3(1f, 1f, 1f), skillPointAnimationTime));

            _skillPoints.Add(skill);
        }

        private void DestroySkillPoint(ScaleController targetSkillPoint)
        {
            if (_skillPoints.Contains(targetSkillPoint) == false || targetSkillPoint == null) return;

            _skillPoints.Remove(targetSkillPoint);
            targetSkillPoint.ChangeScale(Vector3.zero, skillPointAnimationTime);
            TimeUtility.CallWithDelay(skillPointAnimationTime, () => Destroy(targetSkillPoint.gameObject));
        }
        
        public void DestroyLastAddedSkillPoint()
        {
            if (_skillPoints.Count <= 0) return;
            
            var skillPoint = _skillPoints[^1];
            DestroySkillPoint(skillPoint);
        }

        public void DestroyAllPoints()
        {
            var skillsList = new List<ScaleController>(_skillPoints);
            
            foreach (var skillPoint in skillsList)
            {
                if (skillPoint == null) continue;
                DestroySkillPoint(skillPoint);
            }
            
            _skillPoints.Clear();
        }
    }
}