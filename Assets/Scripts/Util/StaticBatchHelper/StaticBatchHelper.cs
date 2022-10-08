using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自动静态合批
/// 使用：1、如果使用自动静态合批必须把模型的可读写给勾选要不然无法合批 
/// 2、将要合批的GameObj放到一个父节点下，然后在父节点挂继承StaticBatchHelper的脚本
/// 3、要合批的物体应该是使用相同材质或贴图的物体来合批
/// 原理：如果在场景中设置Batching Static（静态合批）则AB包的大小就会增大，如果不使用静态合批则会造成Batches和SetPass calls过大影响性能，所以
/// 调用Unity提供的API StaticBatchingUtility.Combine来进行自动合批
/// </summary>
public class StaticBatchHelper : MonoBehaviour
{
    private void Awake()
    {
        StaticBatchingUtility.Combine(gameObject);
    }
}
