using UnityEngine;

public interface Entity
{
    /// <summary>
    /// �����ϴ� ��ü���� ȣ��
    /// </summary>
    /// <param name="damageAmount">������ ��</param>
    /// <returns>�׾����� ��ȯ</returns>
    public abstract bool Attacked(int damageAmount);

    public abstract void Dead();
}