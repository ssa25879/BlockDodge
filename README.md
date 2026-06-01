# BlockDodge

Unity 기반 모바일 회피 게임 프로젝트입니다. 플레이어는 조이스틱으로 캐릭터를 이동하고 회피 버튼을 사용해 날아오는 투사체를 피하면서 최대한 오래 생존하는 것을 목표로 합니다.

## 프로젝트 정보

- 엔진: Unity 6000.3.10f1
- 렌더 파이프라인: Universal Render Pipeline
- 주요 플랫폼: Android
- 주요 씬: `Intro`, `Game`

## 주요 기능

- 조이스틱 기반 캐릭터 이동
- 회피 버튼과 쿨다운 시스템
- 투사체 자동 생성 및 시간 경과에 따른 난이도 증가
- 체력 UI와 게임 오버 처리
- 최고 생존 시간 저장
- 인트로, 게임, 재시작, 종료 흐름
- BGM 및 사망 BGM 재생

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

## 실행 방법

1. Unity Hub에서 Unity 6000.3.10f1 버전으로 프로젝트를 엽니다.
2. `Assets/Scenes/Intro.unity` 또는 `Assets/Scenes/Game.unity` 씬을 엽니다.
3. 에디터에서 Play 버튼을 눌러 실행합니다.

## 이후 수정사항

- 산이나 언덕을 뚫고 지나가는 현상을 수정해야함.

## Git 관리 참고

이 저장소는 Unity 자동 생성 폴더와 빌드 산출물을 제외하도록 `.gitignore`가 설정되어 있습니다. `Library/`, `Logs/`, `UserSettings/`, `*.apk`, `*.aab`, `*.keystore` 등은 Git에 포함하지 않습니다.
