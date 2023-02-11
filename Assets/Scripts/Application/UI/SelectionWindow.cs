using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectionWindow : UIWindow
{
    private int _curIndex;
    public float _yOffset;
    public int _optionCount;            //选项的总数量
    public int _perPageCount;              //每页的选项数量
    private RectTransform _selector;
    private bool _canSelect;
    private Vector2 _orginPos;
    private float _selectInterval = 0.2f;
    private float timer = 0f;


    private int _curPage;                   //当前页（这里页数从0开始）
    private int _curPageSelectionCount;     //当前页选项数量
    private int _maxPage;                   //最大页数量 - 1

    private UnityAction<int, int> PageChaged;
    private UnityAction<int> ConfirmSelect;         //确认选择事件
    private UnityAction<int> MoveSelector;

    public event UnityAction<int> OnConfirmSelect
    {
        add
        {
            ConfirmSelect += value;
        }
        remove
        {
            ConfirmSelect -= value;
        }
    }

    public event UnityAction<int> OnMoveSelector
    {
        add
        {
            MoveSelector += value;
        }
        remove
        {
            MoveSelector -= value;
        }
    }

    public event UnityAction<int,int> OnPageChanged
    {
        add
        {
            PageChaged += value;
        }
        remove
        {
            PageChaged -= value;
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        _curIndex = 0;
        _selector = transform.FindChildTF("img_Selector") as RectTransform;
        _orginPos = _selector.anchoredPosition;
        _canSelect = false;
    }

    public override void OnShow()
    {
        base.OnShow();
        _curIndex = 0;
        _curPage = 0;
        _canSelect = true;
        _selector.anchoredPosition = _orginPos;

    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        if (_optionCount <= 0) return;   //当前没有一个选项

        timer += Time.unscaledDeltaTime;    //这里使用不被缩放影响的间隔时间（因为要暂停游戏）

        if (_canSelect && timer >= _selectInterval)
        {

            if (InputMgr.Instance.GetKeyStay(Consts.K_Up))      //Selector循环上移
            {
                _curIndex = (_curIndex - 1 + _curPageSelectionCount) % _curPageSelectionCount;

                _selector.anchoredPosition = _orginPos + Vector2.up * _yOffset * _curIndex;

                MoveSelector?.Invoke(_curPage * _perPageCount + _curIndex);

                timer = 0f;
            }
            else if (InputMgr.Instance.GetKeyStay(Consts.K_Down))       //Selector循环下移
            {
                _curIndex = (_curIndex + 1) % _curPageSelectionCount;

                _selector.anchoredPosition = _orginPos + Vector2.up * _yOffset * _curIndex;

                MoveSelector?.Invoke(_curPage * _perPageCount + _curIndex);

                timer = 0f;
            }
            else if (InputMgr.Instance.GetKeyDown(Consts.K_Left))       //向左翻页
            {
                _curPage = (_curPage - 1 + _maxPage + 1) % (_maxPage + 1);      //这里循环左移：举个栗子：
                                                                                //一个有2页 当前为0页 最大页为1（这里当前页和最大页都-1主要照顾背包数组下标从零开始）
                                                                                // (0 - 1 + 1 + 1) % (1 + 1) = 1 所以：0页左移 就变成了1页（主要运用模运算）
                                                                                
                _curPageSelectionCount = Mathf.Min(_optionCount - _curPage * _perPageCount, _perPageCount);  //计算当前页数的选项数量

                //这里修正当前页的选项索引（防止物品移除后，选择图标还在原处）
                if (_curIndex >= _curPageSelectionCount) _curIndex = _curPageSelectionCount -1;

                _selector.anchoredPosition = _orginPos + Vector2.up * _yOffset * _curIndex;

                MoveSelector?.Invoke(_curPage * _perPageCount + _curIndex);     

                PageChaged?.Invoke(_curPage, _curPageSelectionCount);
            }
            else if (InputMgr.Instance.GetKeyDown(Consts.K_Right))      //向右翻页（原理同上）
            {
                _curPage = (_curPage + 1) % (_maxPage + 1);

                _curPageSelectionCount = Mathf.Min(_optionCount - _curPage * _perPageCount, _perPageCount);  //计算当前页数的选项数量

                //这里修正当前页的选项索引（防止物品移除后，选择图标还在原处）
                if (_curIndex >= _curPageSelectionCount) _curIndex = _curPageSelectionCount - 1;

                _selector.anchoredPosition = _orginPos + Vector2.up * _yOffset * _curIndex;

                MoveSelector?.Invoke(_curPage * _perPageCount + _curIndex);

                PageChaged?.Invoke(_curPage, _curPageSelectionCount);
            }
        }


        if (InputMgr.Instance.GetKeyDown(Consts.K_Attack))
        {
            ConfirmSelect?.Invoke(_curPage * _perPageCount + _curIndex);
        }
    }
    public override void OnReveal()
    {
        base.OnReveal();
        _canSelect = true;
    }
    public override void OnCover()
    {
        base.OnCover();
        _canSelect = false;
    }

    public override void OnHide()
    {
        base.OnHide();
        _canSelect = false;
    }

    /// <summary>
    /// 设置选项总数
    /// </summary>
    /// <param name="val"></param>
    public void SetCount(int val)
    {
        if (val < 0) return;

        _optionCount = val;

        //求出最大页数-1
        _maxPage = _optionCount % _perPageCount != 0 ? _optionCount / _perPageCount : _optionCount / _perPageCount - 1;

        if (_maxPage == -1) _maxPage = 0;   //背包为空特殊处理
        //尝试修改当前页数（这里主要为了防止最后一页背包的最后一个物品移除时，最大页数减一，需要修改当前页数）
        _curPage = _curPage > _maxPage ? _maxPage : _curPage;

        //当前页数的选项数量
        _curPageSelectionCount = Mathf.Min(_optionCount - _curPage * _perPageCount , _perPageCount);

        //这里修正当前页的选项索引（防止物品移除后，选择图标还在原处）
        if (_curIndex >= _curPageSelectionCount)  _curIndex = _curPageSelectionCount - 1;

        _selector.anchoredPosition = _orginPos + Vector2.up * _yOffset * _curIndex;

        PageChaged?.Invoke(_curPage, _curPageSelectionCount);

    }
}
