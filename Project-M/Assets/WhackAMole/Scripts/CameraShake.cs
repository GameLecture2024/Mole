using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // 어디서든 접근할 수 있는 싱글톤 인스턴스
    public static CameraShake Instance { get; private set; }

    private Vector3 originalPos;
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        // 싱글톤 초기화 및 중복 방지
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 카메라 진동 효과를 실행합니다.
    /// </summary>
    /// <param name="duration">진동 지속 시간 (초)</param>
    /// <param name="magnitude">진동 세기 (반경)</param>
    public void Shake(float duration, float magnitude)
    {
        // 이미 진동 중이라면 기존 진동을 멈추고 새로 시작 (위치 틀어짐 방지)
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            transform.localPosition = originalPos;
        }
        else
        {
            // 진동 시작 전 카메라의 원래 로컬 위치 저장
            originalPos = transform.localPosition;
        }

        shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // 반지름이 1인 원 내부의 랜덤한 위치 생성 후 세기(magnitude) 곱하기
            // 3D 게임에서 Z축까지 흔들고 싶다면 Random.insideUnitSphere를 사용하세요.
            Vector2 randomPoint = Random.insideUnitCircle * magnitude;

            transform.localPosition = new Vector3(
                originalPos.x + randomPoint.x,
                originalPos.y + randomPoint.y,
                originalPos.z
            );

            elapsed += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 진동이 끝나면 원래 위치로 정확히 복구
        transform.localPosition = originalPos;
        shakeCoroutine = null;
    }
}
