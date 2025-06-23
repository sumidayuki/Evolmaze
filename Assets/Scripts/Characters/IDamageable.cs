/// <summary>
/// ダメージを受けるオブジェクトにこのインターフェイスを実装することで、このインターフェイスを経由してダメージ処理を行うことができます。
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// ダメージを受けたときの処理
    /// </summary>
    /// <param name="amount">ダメージ量</param>
    public void Damage(float amount);
}
