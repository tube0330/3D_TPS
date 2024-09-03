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
- DeleteAll: 모든 키 값 삭제
- DeleteKey: 특정 키 값 삭제
- Get자료형("키", value);
- Set자료형("키", value);

- HasKey: 해당 키 존재 유무
- Save: 변경된 키 값을 물리적인 저장공간에 저장

# 240729
1. GameData는 고전적인 방식.
Attribute를 사용해 자동으로 생성하기 위해 GameData 대신 GameDataObject 스크립트 생성
2. FireCtrl에서 Raycast를 사용해 적 태그를 가지고 있다면 자동으로 총알 발사
3. 적들이 Barrel이나 Wall 같은 장애물에 가려져 있으면 총알 발사 제한
4. FollowCam_Camera에서 Raycast를 쐈을 때 Player가 아닌 Gameobject가 맞으면 카메라 위치 이동

# 240730
1. Enemy Character의 지능화 된 AI
- 발사 거리 조건만 만족하면 총알 발사 -> Enemy와 Player 사이에 장애물이 있을 때 총알 발사 제한
2. Enemy Character에 추적 사정거리와 시야각을 시각적으로 표시
- DrawWireDisc와 DrawSolidArc 함수 사용으로 Scene View 에서 시각적으로 구성
3. 라이트매핑(Light Mapping) 및 라이트프로브(Light Probe)
4. Scene 합치기(Scene Merge)

# 240805
1. 씬 전환 시 Fade in 로직
씬의 사이즈가 크거나 내려받아야 할 데이터가 많은 경우 씬을 로드하기 까지 시간이 오래 걸림
이때 다음으로 넘어가지 않고 그냥 멈춰선 느김을 받기 때문에 중간에 게임 튜토리얼 이미지를 넣거나 검은색에서 투명한 색으로 서서히 바뀌고 화면이 로드되면 다시 밝게 처리
2. 플레이어 카 만들기
WASD로 자동차 조종하기
F키 눌러서 헤드라이트 그고 켜기
자동차 후미등 후진할 때 켜기
1인칭 캐릭터로 자동차 근처에 가면 타고 운전 할 때는 3인칭 카메라
Q를 누르면 자동차에서 내리고 자동차 정지
-> 1인칭 3인칭 반복	

================================================================
# 240903
1. InputSystem으로 변경
2. PUN2 이용해 네트워크 게임으로 변경
- Random Match Making 방식으로 접속
- 공개방, 방목록 리스트 표시
- 로그 UI로 접속했던 ID와 접속한 ID 표시
- 접속자 수 / 최대 접속자 수
- 플레이어끼리는 대전을 하지 않고 Enemy만 공격
- Kill Count 기록
- Enemy는 가장 가까운 player를 공격