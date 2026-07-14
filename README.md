# BlockDodge

Unity 기반 모바일 생존형 회피 게임 프로젝트입니다. 플레이어는 조이스틱으로 캐릭터를 이동하고, 회피 버튼을 사용해 대포에서 발사되는 투사체를 피하면서 최대한 오래 생존하는 것을 목표로 합니다.

## 프로젝트 정보

- 엔진: Unity 6000.3.10f1
- 렌더 파이프라인: Universal Render Pipeline
- 주요 플랫폼: Android
- 주요 씬: `Intro`, `Game`

## 주요 기능

- 조이스틱 기반 캐릭터 이동
- 회피 버튼, 회피 지속시간, 쿨다운, 회피 중 무적 판정
- HP 3 기반 피격 및 게임 오버 처리
- 생존 시간 UI와 최고 생존 시간 저장
- 시간 기반 단계형 난이도 관리
  - Easy, Normal, Hard, Very Hard
  - 발사 간격, 탄속 상한, 활성 포대 수, 탄막 패턴 조절
- 탄막 패턴
  - 단발
  - 연속 발사
  - 확산 발사
- 플레이 중 현재 난이도 표시
- 외곽 벽, 산 차단벽, 투명 바닥 콜라이더를 통한 맵/탄환 이탈 방지
- 테스트용 데미지 무시 토글

## 프로젝트 구조

```text
Assets/
  Images/        UI와 아이콘 이미지
  Music/         배경 음악 리소스
  Prefab/        Bullet, Bullet Spawner 프리팹
  Scenes/        Intro, Game 씬
  Scripts/       게임 로직 스크립트
  Settings/      URP 및 렌더링 설정
Packages/        Unity 패키지 매니페스트
ProjectSettings/ Unity 프로젝트 설정
```

## 주요 스크립트

- `GameManager`: 생존 시간, 점수 표시, 게임 오버 흐름 관리
- `DifficultyManager`: 시간 기반 난이도 단계와 UI 표시 관리
- `BulletSpawner`: 탄환 속도, 발사 간격, 활성 상태, 탄막 패턴 관리
- `Bullet`: 플레이어 및 벽 충돌 시 피격/제거 처리
- `PlayerController`: 이동, 회피, HP, 테스트용 무적 토글 관리

## 실행 방법

1. Unity Hub에서 Unity 6000.3.10f1 버전으로 프로젝트를 엽니다.
2. `Assets/Scenes/Intro.unity` 또는 `Assets/Scenes/Game.unity` 씬을 엽니다.
3. 에디터에서 Play 버튼을 눌러 실행합니다.

## Git 관리 참고

이 저장소는 Unity 자동 생성 폴더, 빌드 산출물, 로컬 백업, 서명 파일을 제외하도록 `.gitignore`가 설정되어 있습니다.

Git에 포함하지 않는 주요 항목:

- `Library/`, `Temp/`, `Logs/`, `UserSettings/`
- `Backups/`, `*.bak`
- `*.apk`, `*.aab`
- `*.keystore`, `*.p12`, `*.pfx`
- 로컬 에이전트 설정 폴더
