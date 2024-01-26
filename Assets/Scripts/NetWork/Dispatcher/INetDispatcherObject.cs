using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INetDispatcherObject
{
    void HandleNetWorkNotice<T>(string netOpCode, T args);
}
