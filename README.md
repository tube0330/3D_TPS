# 3DGame_TPS
1. 총알 오브젝트 풀링

2. 배럴이 총알에 5번 맞으면 주위에 있는 배럴도 영향 받도록 구현  적 Enemy만들기

1. 패트롤 patroll 기능구현 walk
2. 패트롤하다 플레이어를 발견하면 뛰어오기
3. Mecanim 방식으로 공격(총알쏘기) Reload 기능으로 Avata 만들어서 구현
4. enemy 총알 오브젝트 풀링

# 240724
1. 사용자 정의 Gizmo
MyGizmo 클래스에 원하는 아이콘 이미지 넣기
spawnpoint마다 다른 아이콘으로 patroll point 다른 아이콘
2. 총기 교체 버튼을 만들어 아이콘 변경, 총마다 소리 변경, 버튼(UI)을 누르면 총알 발사 막기(using EventSystem)
3. 일시정지 버튼 눌러 시간 중지: 싱글 게임에만 해당
canvasGroup -> blockRayCast 활성화 비활성화에 따라 UI 이벤트를 받을건지 여부

# 240725
1. Patroll Point 랜덤하게 움직이기
2. Barrel 폭발 시 반경 안에 있는 Enemy는 Die
3. for문 대신 delegate event 대리자와 이벤트로 Enemy에게 playerDie 알리기
4. killcount가 게임이 종료되어도 다시 시작하면 이전의 killcount를 불러옴

# 240726
1. playerPrefs를 활용한 데이터 저장

함수종류
DeleteAll: 모든 키 값 삭제
DeleteKey: 특정 키 값 삭제
Get자료형("키", value);
Set자료형("키", value);

HasKey: 해당 키 존재 유무
Save: 변경된 키 값을 물리적인 저장공간에 저장
