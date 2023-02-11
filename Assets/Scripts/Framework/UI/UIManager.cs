using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : SingletonBase<UIManager>
{
    private RectTransform _canvas;
    private LinkedList<BasePanel> _panelStack;
    private Dictionary<string, BasePanel> _panelDic;
    private string _uiPath;
    //TODO:可能需要补充UI的层级相关
    private UIManager()
    {
        _uiPath = Consts.P_UI;
        _canvas = ResourceMgr.Instance.LoadAsset<GameObject>(_uiPath + "Canvas").transform as RectTransform;
        GameObject.DontDestroyOnLoad(_canvas);
        _panelStack = new LinkedList<BasePanel>();
        _panelDic = new Dictionary<string, BasePanel>();
        MonoMgr.Instance.AddUpateAction(OnUpdate);
    }

    public float CanvasWidth => _canvas.rect.width;
    public float CanvasHeight => _canvas.rect.height;

    public void Push<T>(UnityAction<T> callback = null) where T :BasePanel
    {
        if (_panelStack.Count > 0)
        {
            _panelStack.First.Value.OnCover();
        }

        string panelName = typeof(T).Name;
        if(_panelDic.ContainsKey(panelName))
        {
            T panel = _panelDic[panelName] as T;
            (panel.transform as RectTransform).offsetMax = Vector2.zero;
            panel.transform.localScale = Vector3.one;
            if (_panelStack.First.Value != panel)
            {
                _panelStack.Remove(_panelDic[panelName]);
                _panelStack.AddFirst(panel);
                //重新设置父对象，将面板显示最前面
                panel.transform.SetParent(null);
                panel.transform.SetParent(_canvas);
            }
            callback?.Invoke(panel);
            panel.OnOpen();
            return;
        }

        PoolMgr.Instance.PopObjAsync<GameObject>(_uiPath + panelName, (obj) =>
         {
             T panel = obj.GetOrAddComponent<T>();
             _panelDic.Add(panelName, panel);
             obj.transform.SetParent(_canvas);

             (panel.transform as RectTransform).offsetMax = Vector2.zero;
             panel.transform.localScale = Vector3.one;
             _panelStack.AddFirst(panel);

             callback?.Invoke(panel);
             panel.OnOpen();

         });

    }

    public void Pop()
    {
        if (_panelStack.Count == 0) return;

        var panel = _panelStack.First;
        _panelStack.RemoveFirst();
        _panelDic.Remove(panel.Value.GetType().Name);
        panel.Value.OnHide();

        if(_panelStack.Count > 0)
        {
            _panelStack.First.Value.OnReveal();
        }
    }

    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (_panelDic.ContainsKey(panelName))
        {
            return _panelDic[panelName] as T;
        }
        return null;
    }
    public void ShowDialog(Vector2 screenPoint, DialogNode node)
    {
        Push<DialogPanel>((panel) =>
        {
            panel.SetData(node, screenPoint);
            //TODO: 这里改变分辨率，会改变画布的宽高，需要重新计算（后续可能会写一个修改分辨率的事件）
            RectTransformUtility.ScreenPointToLocalPointInRectangle(panel.transform as RectTransform, screenPoint, null, out var dialogPos);
            //Debug.Log(_canvas.sizeDelta);
            //限制对话框的x轴范围，防止对话框超出屏幕外
            dialogPos.x = Mathf.Clamp(dialogPos.x,
                -_canvas.sizeDelta.x / 2 + panel.width / 2 + _canvas.sizeDelta.x * 0.05f,
                _canvas.sizeDelta.x / 2 - panel.width / 2 - _canvas.sizeDelta.x * 0.05f);
            panel.DialogBox.localPosition = dialogPos;

        });
    }

    public void OnUpdate()
    {
        if (_panelStack.Count == 0) return;
        _panelStack.First.Value.OnUpdate();
        //Debug.Log($"{_panelStack.First.Value.gameObject.name}");
    }

    public void Clear()
    {

    }
}
