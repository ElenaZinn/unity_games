using UnityEngine;
using System.Collections;
using System.Collections.Generic;
///
///这个脚本旨在创建可以供玩家行动的隐形砖块。游戏的机制是探索允许旋转的3D关卡，这意味着角色在不同时刻有可能处于不同的深度坐标，有可能会无法保持始终立足在我们创建的实体平台之上。
///在实际游玩中我们看到的角色是在2D平面中行动的，角色与不同深度的砖块看起来是在同一个平面上的。但是一旦它们的深度值不同，我们就需要创建供角色落脚的隐形平台。
///当时机成熟时，我们就会将玩家转移到距离最近的实体平台，这样我们就不会暴露我们游戏的机制了。
///
public class PlayerManager : MonoBehaviour
{
    //玩家移动及朝向
    private PlayerMove fezMove;
    public FacingDirection facingDirection;
    public GameObject Player;
    //设置旋转角度
    private float degree = 0;
    //实体砖块
    public Transform Level;
    //建筑砖块
    public Transform Building;
    //隐形砖块
    public GameObject InvisiCube;
    private List<Transform> InvisiList = new List<Transform>();
    //上一帧玩家的朝向，深度，用来避免无意义的重复构建隐形砖块位置
    private FacingDirection lastfacing;
    private float lastDepth = 0f;
    //单元尺寸，所有立方体砖块的尺寸应与此保持一致
    public float WorldUnits = 1.000f;

    void Start()
    {
        facingDirection = FacingDirection.Front;
        fezMove = Player.GetComponent<PlayerMove>();
        UpdateLevelData(true);
    }

    void Update()
    {
        //控制玩家当前深度逻辑
        //如果玩家当前处于一个隐形平台之上，尝试转移到实体平台之上，这可以避免旋转时暴露我们的逻辑
        //尽可能转移到距离摄像机最近的平台，这可以使我们的旋转时的效果更加自然
        if (!fezMove._jumping)
        {
            bool updateData = false;
            if (OnInvisiblePlatform())
                if (MovePlayerDepthToClosestPlatform())
                    updateData = true;
            if (MoveToClosestPlatformToCamera())
                updateData = true;
            if (updateData)
                UpdateLevelData(false);
        }

        //旋转操作
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //当进行旋转操作时，必须保持玩家处于实体平台之上，否则旋转之后我们有可能会处于半空之中。
            if (OnInvisiblePlatform())
            {
                //MoveToClosestPlatform();
                MovePlayerDepthToClosestPlatform();
            }
            lastfacing = facingDirection;
            facingDirection = RotateDirectionRight();
            degree -= 90f;
            UpdateLevelData(false);
            fezMove.UpdateToFacingDirection(facingDirection, degree);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (OnInvisiblePlatform())
            {
                //MoveToClosestPlatform();
                MovePlayerDepthToClosestPlatform();
            }
            lastfacing = facingDirection;
            facingDirection = RotateDirectionLeft();
            degree += 90f;
            UpdateLevelData(false);
            fezMove.UpdateToFacingDirection(facingDirection, degree);
        }
    }

    /// 摧毁之前的隐形平台
    /// 根据玩家的朝向和当前的2D平面来创建新的隐形平台

    private void UpdateLevelData(bool forceRebuild)
    {
        //If facing direction and depth havent changed we do not need to rebuild
        if (!forceRebuild)
            if (lastfacing == facingDirection && lastDepth == GetPlayerDepth())
                return;
        foreach (Transform tr in InvisiList)
        {
            //Move obsolete invisicubes out of the way and delete

            tr.position = Vector3.zero;
            Destroy(tr.gameObject);

        }
        InvisiList.Clear();
        float newDepth = 0f;

        newDepth = GetPlayerDepth();
        CreateInvisicubesAtNewDepth(newDepth);


    }
    ///
    /// 判断玩家是否站在一个隐形平台之上
    ///
    private bool OnInvisiblePlatform()
    {
        foreach (Transform item in InvisiList)
        {

            if (Mathf.Abs(item.position.x - fezMove.transform.position.x) < WorldUnits && Mathf.Abs(item.position.z - fezMove.transform.position.z) < WorldUnits)
                if (fezMove.transform.position.y - item.position.y <= WorldUnits + 0.2f && fezMove.transform.position.y - item.position.y > 0)
                    return true;



        }
        return false;
    }
    ///
    /// 将玩家传送到与摄像机高度一致，且距离最近的砖块
    /// 仅支持单元尺度为1的砖块
    ///
    private bool MoveToClosestPlatformToCamera()
    {
        bool moveCloser = false;
        foreach (Transform item in Level)
        {
            if (facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back)
            {
                if (Mathf.Abs(item.position.x - fezMove.transform.position.x) < WorldUnits + 0.1f)
                {
                    if (fezMove.transform.position.y - item.position.y <= WorldUnits + 0.2f && fezMove.transform.position.y - item.position.y > 0 && !fezMove._jumping)
                    {
                        if (facingDirection == FacingDirection.Front && item.position.z < fezMove.transform.position.z)
                            moveCloser = true;
                        if (facingDirection == FacingDirection.Back && item.position.z > fezMove.transform.position.z)
                            moveCloser = true;
                        if (moveCloser)
                        {
                            fezMove.transform.position = new Vector3(fezMove.transform.position.x, fezMove.transform.position.y, item.position.z);
                            return true;
                        }
                    }
                }

            }
            else
            {
                if (Mathf.Abs(item.position.z - fezMove.transform.position.z) < WorldUnits + 0.1f)
                {
                    if (fezMove.transform.position.y - item.position.y <= WorldUnits + 0.2f && fezMove.transform.position.y - item.position.y > 0 && !fezMove._jumping)
                    {
                        if (facingDirection == FacingDirection.Right && item.position.x > fezMove.transform.position.x)
                            moveCloser = true;
                        if (facingDirection == FacingDirection.Left && item.position.x < fezMove.transform.position.x)
                            moveCloser = true;
                        if (moveCloser)
                        {
                            fezMove.transform.position = new Vector3(item.position.x, fezMove.transform.position.y, fezMove.transform.position.z);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }


    /// 查询隐形砖块列表
    private bool FindTransformInvisiList(Vector3 cube)
    {
        foreach (Transform item in InvisiList)
        {
            if (item.position == cube)
                return true;
        }
        return false;

    }
    /// 查询实体砖块列表
    private bool FindTransformLevel(Vector3 cube)
    {
        foreach (Transform item in Level)
        {
            if (item.position == cube)
                return true;
        }
        return false;

    }
    /// 判断相机和砖块之间是否有其他的建筑方块
    private bool FindTransformBuilding(Vector3 cube)
    {
        foreach (Transform item in Building)
        {
            if (facingDirection == FacingDirection.Front)
            {
                if (item.position.x == cube.x && item.position.y == cube.y && item.position.z < cube.z)
                    return true;
            }
            else if (facingDirection == FacingDirection.Back)
            {
                if (item.position.x == cube.x && item.position.y == cube.y && item.position.z > cube.z)
                    return true;
            }
            else if (facingDirection == FacingDirection.Right)
            {
                if (item.position.z == cube.z && item.position.y == cube.y && item.position.x > cube.x)
                    return true;

            }
            else
            {
                if (item.position.z == cube.z && item.position.y == cube.y && item.position.x < cube.x)
                    return true;

            }
        }
        return false;

    }

    /// 当玩家跳到一个隐形平台上时，将玩家转移到高度相同距离最近的实体平台之上
    private bool MovePlayerDepthToClosestPlatform()
    {
        foreach (Transform item in Level)
        {

            if (facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back)
            {
                if (Mathf.Abs(item.position.x - fezMove.transform.position.x) < WorldUnits + 0.1f)
                    if (fezMove.transform.position.y - item.position.y <= WorldUnits + 0.2f && fezMove.transform.position.y - item.position.y > 0)
                    {

                        fezMove.transform.position = new Vector3(fezMove.transform.position.x, fezMove.transform.position.y, item.position.z);
                        return true;

                    }
            }
            else
            {
                if (Mathf.Abs(item.position.z - fezMove.transform.position.z) < WorldUnits + 0.1f)
                    if (fezMove.transform.position.y - item.position.y <= WorldUnits + 0.2f && fezMove.transform.position.y - item.position.y > 0)
                    {

                        fezMove.transform.position = new Vector3(item.position.x, fezMove.transform.position.y, fezMove.transform.position.z);
                        return true;
                    }
            }
        }
        return false;

    }
    ///创建隐形平台逻辑
    private Transform CreateInvisicube(Vector3 position)
    {
        GameObject go = Instantiate(InvisiCube) as GameObject;

        go.transform.position = position;

        return go.transform;
    }

    private void CreateInvisicubesAtNewDepth(float newDepth)
    {

        Vector3 tempCube = Vector3.zero;
        foreach (Transform child in Level)
        {

            if (facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back)
            {
                tempCube = new Vector3(child.position.x, child.position.y, newDepth);
                if (!FindTransformInvisiList(tempCube) && !FindTransformLevel(tempCube) && !FindTransformBuilding(child.position))
                {

                    Transform go = CreateInvisicube(tempCube);
                    InvisiList.Add(go);
                }

            }
            //仅改变当前深度坐标，对其他坐标不做出改变
            else if (facingDirection == FacingDirection.Right || facingDirection == FacingDirection.Left)
            {
                tempCube = new Vector3(newDepth, child.position.y, child.position.z);
                if (!FindTransformInvisiList(tempCube) && !FindTransformLevel(tempCube) && !FindTransformBuilding(child.position))
                {

                    Transform go = CreateInvisicube(tempCube);
                    InvisiList.Add(go);
                }

            }


        }


    }
    /// 当需要使玩家返回起始处时
    public void ReturnToStart()
    {
        UpdateLevelData(true);
    }
    /// 根据玩家朝向判断并返回当前深度坐标值
    private float GetPlayerDepth()
    {
        float ClosestPoint = 0f;

        if (facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back)
        {
            ClosestPoint = fezMove.transform.position.z;

        }
        else if (facingDirection == FacingDirection.Right || facingDirection == FacingDirection.Left)
        {
            ClosestPoint = fezMove.transform.position.x;
        }


        return Mathf.Round(ClosestPoint);

    }


    /// 当我们向右旋转后重新判断玩家朝向
    private FacingDirection RotateDirectionRight()
    {
        int change = (int)(facingDirection);
        change++;
        //Our FacingDirection enum only has 4 states, if we go past the last state, loop to the first
        if (change > 3)
            change = 0;
        return (FacingDirection)(change);
    }
    /// 当我们向左旋转后重新判断玩家朝向
    private FacingDirection RotateDirectionLeft()
    {
        int change = (int)(facingDirection);
        change--;
        if (change < 0)
            change = 3;
        return (FacingDirection)(change);
    }

}
//枚举，用于判断当前朝向
public enum FacingDirection
{
    Front = 0,
    Right = 1,
    Back = 2,
    Left = 3
}