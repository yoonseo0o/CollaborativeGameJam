using UnityEngine;

public interface Entity
{
    /// <summary>
    /// 공격하는 객체에서 호출
    /// </summary>
    /// <param name="damageAmount">데미지 양</param>
    /// <returns>죽었는지 반환</returns>
    public abstract bool Attacked(int damageAmount);

    public abstract void Dead();
}