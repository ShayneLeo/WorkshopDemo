using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
public class BeatEffect : MonoBehaviour
{

    public float m_bigScale = 1.1f;
    public float m_normalScale = 1f;
    public float m_smallScale = 0.9f;
    private Sequence m_sequenceScale;
    public bool m_isHome = false;
    private void Awake()
    {
        m_sequenceScale = DOTween.Sequence();
        m_sequenceScale.SetAutoKill(false);
        if (m_isHome)
        {
            m_sequenceScale
                .Append(transform.DOScale(new Vector3(m_bigScale, 1f, m_bigScale), 0.5f))
                .Append(transform.DOScale(new Vector3(m_smallScale, 1f, m_smallScale), 0.1f))
                .Append(transform.DOScale(new Vector3(m_normalScale, 1f, m_normalScale), 0.1f));
        }
        else
        {
            m_sequenceScale
                .Append(transform.DOScale(new Vector3(m_bigScale, m_bigScale, 1f), 0.5f))
                .Append(transform.DOScale(new Vector3(m_smallScale, m_smallScale, 1f), 0.1f))
                .Append(transform.DOScale(new Vector3(m_normalScale, m_normalScale, 1f), 0.1f));
        }
        m_sequenceScale.SetLoops(-1);
        m_sequenceScale.Pause();
    }
    private void OnEnable()
    {
        m_sequenceScale.Play();
    }
}
