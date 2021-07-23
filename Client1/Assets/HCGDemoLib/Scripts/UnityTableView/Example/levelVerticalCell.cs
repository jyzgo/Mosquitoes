using Assets.Scripts.TableView;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Assets.Scripts.Example
{
    public class levelVerticalCell : TableViewCell
    {
        
        public  LevelHorizontalCell prefab;
        List<LevelHorizontalCell> _hCells = new List<LevelHorizontalCell>();

        private new void Awake()
        {
            base.Awake();
            for (int i = 0; i < mulExampleViewController.eachRowCellCount; i++)
            {
                var gb = Instantiate<LevelHorizontalCell>(prefab);
                gb.transform.SetParent(transform);
                _hCells.Add(gb);
            }

        }

        public override string ReuseIdentifier
        {
            get { return "VerticalCellID"; }
        }

        public override void SetHighlighted()
        {
            print("CellSetHighlighted : " + RowNumber);
        }

        public override void SetSelected()
        {
            print("CellSetSelected : " + RowNumber);
        }


        public override void Display()
        {
            int totalRow = mulExampleViewController.cellTotalNumber /mulExampleViewController.eachRowCellCount;

            for (int i = 0; i < _hCells.Count; i++)
            {
                _hCells[i].UpdateCell(RowNumber * mulExampleViewController.eachRowCellCount + i + 1);
            }


            if (RowNumber < totalRow)
            {
                foreach (var v in _hCells)
                {
                    v.gameObject.SetActive(true);
                }
            }
            else
            {
                int activeNum = mulExampleViewController.cellTotalNumber - totalRow * mulExampleViewController.eachRowCellCount;
                //    _hCells[i].gameObject.SetActive(i < activeNum);
                //}
            }

        }
    }
}