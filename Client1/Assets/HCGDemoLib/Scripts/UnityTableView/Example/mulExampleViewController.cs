using Assets.Scripts.TableView;
using UnityEngine;

namespace Assets.Scripts.Example
{
    public class mulExampleViewController : MonoBehaviour, ITableViewDataSource, ITableViewDelegate
    {
        public TableView.TableView tableView;

        public static int cellTotalNumber = InitMgr.MAX_LEVEL_INDEX;
        public static int eachRowCellCount = 4;
        public string MUL_ID = "MulExampleTableViewCellReuseIdentifier";
        //43
        //Each row would be 8;


        public GameObject tableviewCellPrefab;

        void Start()
        {
            tableView.Delegate = this;
            tableView.DataSource = this;

            //GameObject prefab = Resources.Load("MulExampleTableViewCell") as GameObject;
            tableView.RegisterPrefabForCellReuseIdentifier(tableviewCellPrefab, MUL_ID);
        }

        public virtual int NumberOfRowsInTableView(TableView.TableView tableView)
        {
            int num = (int)Mathf.Ceil(((float)cellTotalNumber) / ((float)eachRowCellCount));
            return num;
        }

        public static float RowSize = 200f;

        public virtual float GetRowSize()
        {
            return RowSize;
        }

        public float SizeForRowInTableView(TableView.TableView tableView, int row)
        {
            //print("Row Size " + RowSize); 
            return GetRowSize();// Random.Range(50.0f, 200.0f);
        }

        public TableViewCell CellForRowInTableView(TableView.TableView tableView, int row)
        {
            TableViewCell cell = tableView.ReusableCellForRow(MUL_ID, row);
            cell.name = "Cell " + row;
            return cell;
        }

        public void TableViewDidHighlightCellForRow(TableView.TableView tableView, int row)
        {
            print("TableViewDidHighlightCellForRow : " + row);
        }

        public void TableViewDidSelectCellForRow(TableView.TableView tableView, int row)
        {
            print("TableViewDidSelectCellForRow : " + row);
        }
    }
}