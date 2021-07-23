using Assets.Scripts.TableView;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Assets.Scripts.Example
{
    public class mulExampleTableViewCell : TableViewCell
    {
        
        public mulHorizontalCell prefab;
        List<mulHorizontalCell> _hCells = new List<mulHorizontalCell>();

        private void Awake()
        {
            base.Awake();
            for (int i = 0; i < mulExampleViewController.eachRowCellCount; i++)
            {
                var gb = Instantiate<mulHorizontalCell>(prefab);
                gb.transform.parent = transform;
                _hCells.Add(gb);
            }

        }
        public void Start()
        {
            
        }

        public override string ReuseIdentifier
        {
            get { return "ExampleTableViewCellReuseIdentifier"; }
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

            if(RowNumber < totalRow)
            {
                foreach(var v in _hCells)
                {
                    v.gameObject.SetActive(true);
                }
            }else
            {
                int activeNum = mulExampleViewController.cellTotalNumber - totalRow * mulExampleViewController.eachRowCellCount;
                for (int i = 0; i < _hCells.Count; i++)
                {
                    _hCells[i].gameObject.SetActive(i < activeNum);
                }
            }
            
        }
    }
}