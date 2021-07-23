using Assets.Scripts.TableView;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Assets.Scripts.Example
{
    public class CharVerticalCell: TableViewCell
    {
        
        public CharHorizontalCell prefab;
        List<CharHorizontalCell> _hCells = new List<CharHorizontalCell>();

        private new void Awake()
        {
            base.Awake();
            for (int i = 0; i < CharBuyTableview.eachRowCellCount; i++)
            {
                var gb = Instantiate<CharHorizontalCell>(prefab);
                gb.transform.SetParent(transform);
                _hCells.Add(gb);
            }

        }

        public override string ReuseIdentifier
        {
            get { return "CharVerticalCellID"; }
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
            //print("dis play " + skinMgr.skinCount);
            int totalRow = skinMgr.skinCount / CharBuyTableview.eachRowCellCount;

            for (int i = 0; i < _hCells.Count; i++)
            {
                _hCells[i].UpdateCell(RowNumber * CharBuyTableview.eachRowCellCount + i);
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
                int activeNum = skinMgr.skinCount - totalRow * CharBuyTableview.eachRowCellCount;
                //    _hCells[i].gameObject.SetActive(i < activeNum);
                //}
            }

        }
    }
}