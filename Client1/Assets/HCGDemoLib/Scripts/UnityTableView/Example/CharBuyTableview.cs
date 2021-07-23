using Assets.Scripts.Example;
using Assets.Scripts.TableView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharBuyTableview : mulExampleViewController
{
    int cellNum = 0;
    private void Awake()
    {
        cellNum = skinMgr.skinCount / 2;
        
    }
    public new static int cellTotalNumber = InitMgr.MAX_CHAR_NUM;
    public new static int eachRowCellCount = 2;
    public new static float RowSize = 450;
    public override float GetRowSize()
    {
        return 500f;
    }

    public override int NumberOfRowsInTableView(TableView tableView)
    {
        return cellNum;
    }

}
