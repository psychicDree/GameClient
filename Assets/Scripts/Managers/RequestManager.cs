using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class RequestManager : BaseManager
{
    public RequestManager(GameFacade facade) : base(facade) { }
    private Dictionary<ActionCode, BaseRequest> _requestsDict = new Dictionary<ActionCode, BaseRequest>();

    public void AddRequest(ActionCode actionCode, BaseRequest baseRequest)
    {
        _requestsDict.Add(actionCode,baseRequest);
    }
    public void RemoveRequest(ActionCode actionCode)
    {
        _requestsDict.Remove(actionCode);
    }
    public override void OnInit()
    {
        
    }

    public void HandleResponse(ActionCode actionCode, string data)
    {
        BaseRequest request = _requestsDict.TryGet<ActionCode, BaseRequest>(actionCode);
        request.OnResponse(data);
    }

}