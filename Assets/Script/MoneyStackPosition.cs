using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyStackPosition : MonoBehaviour
{
    [SerializeField] Transform xLeft,xRight,yBot,yTop,zFront,zBack;
    [SerializeField] GameObject prefab;
    private List<float> posX,posY,posZ;
    private List<MoneyStackPos> moneyPos;
    private const int MONEYWIDTH = 6;
    private const int MONEYBREADTH = 3;
    private const int MONEYHEIGHT = 6;

    float xMin => xLeft.position.x;
    float xMax => xRight.position.x;
    float yMin => yBot.position.y;
    float yMax => yBot.position.y;
    float zMin => zFront.position.z;
    float zMax => zBack.position.z;
    private bool isFull;

    private void Awake()
    {
        posX = new List<float>();
        posY = new List<float>();
        posZ = new List<float>();
        moneyPos = new List<MoneyStackPos>();
        posX = CalPosByLenght( xMin, xMax, MONEYWIDTH);
        posY = CalPosByLenght( yMin, yMax, MONEYHEIGHT);
        posZ = CalPosByLenght( zMin, zMax, MONEYBREADTH);
        moneyPos = MoneyPosCal(posX, posY, posZ);
    }
     List<float> CalPosByLenght(float min, float max, int lenght)
    {
        var pos = new List<float>();
        float step = (max - min)/(lenght-1);
        for(int i =0; i< lenght;i++)
        {
            pos.Add(min + i * step);
        }
        return pos;
    }
    List<MoneyStackPos> MoneyPosCal(List<float> posX, List<float> posY, List<float> posZ)
    {
        var pos = new List<MoneyStackPos>();
        for(int i =0;i< posY.Count; i++)
        {
            for(int o = posX.Count -1;o>=0; o--)
            {
                for(int p =0;p<posZ.Count;p++)
                {
                    var moneyPos = new Vector3(o, i, p);
                    pos.Add(new MoneyStackPos(moneyPos));
                    Instantiate(gameObject).transform.position = moneyPos;
                }
            }
        }
        return pos;
    }
}

public class MoneyStackPos
{
    Vector3 pos;
    bool assigned;
    public MoneyStackPos(Vector3 pos)
    {
        this.pos = pos;
    }
    public Vector3 Pos => pos;
    public bool IsAssigned => assigned;
}
