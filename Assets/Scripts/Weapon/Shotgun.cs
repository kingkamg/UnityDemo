using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 霰弹枪控制层
/// </summary>
public class Shotgun : LikeGunControllerBase
{
    private ShotgunView m_ShotgunView;      //视图层引用
    private int bulletCount = 5;            //载弹量

    protected override void Init()
    {
        m_ShotgunView = (ShotgunView)M_GunViewBase;
    }
    protected override void PlayEffect()
    {
        //枪口火焰
        GameObject temp = Instantiate(m_ShotgunView.M_FireEffect, m_ShotgunView.M_MuzzlePos);
        temp.GetComponent<ParticleSystem>().Play();
        StartCoroutine(DelayDestroy(temp, 2f));
    }
    protected override void Shot()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            StartCoroutine(DelayShotBullet());
        }
        Durable--;
    }
    /// <summary>
    /// 延迟霰弹枪发射子弹
    /// </summary>
    private IEnumerator DelayShotBullet()
    {
        yield return new WaitForSeconds(0.02f);
        GameObject bullet = Instantiate(m_ShotgunView.Bullet, m_ShotgunView.M_MuzzlePos.position
            , Quaternion.identity);
        ShotgunBullet sbt = bullet.GetComponent<ShotgunBullet>();
        sbt.Flight(m_ShotgunView.M_MuzzlePos.forward, 3000, M_Damage / bulletCount, Hit);
    }
    /// <summary>
    /// 延迟销毁目标;
    /// </summary>
    private IEnumerator DelayDestroy(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(go);
    }
    /// <summary>
    /// 拉栓音效,Event调用;
    /// </summary>
    public void ReLoadAudio()
    {
        AudioSource.PlayClipAtPoint(m_ShotgunView.ReLoad, m_ShotgunView.M_MuzzlePos.position);
    }
    /// <summary>
    /// 弹壳出仓动作,Event调用;
    /// </summary>
    public void ThrowShell()
    {
        GameObject tempShell = Instantiate(m_ShotgunView.Shell, m_ShotgunView.ShellPos.position, Quaternion.identity);
        tempShell.GetComponent<Rigidbody>().AddForce(m_ShotgunView.ShellPos.up * Random.Range(80f, 105f));
        StartCoroutine(DelayDestroy(tempShell, 3f));
    }
}
