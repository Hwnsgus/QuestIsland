🗺️ Unity Quest & Dialogue SystemScriptableObject를 활용한 확장 가능한 퀘스트 시스템 구현다양한 타입의 퀘스트(사냥, 수집, 이동)를 유연하게 생성하고 관리할 수 있는 모듈형 RPG 퀘스트 시스템입니다.📸 Project Overview (Gameplay)<div align="center"><img src="Images/주무대.jpg" alt="Game World Overview" width="800"><p><em>▲ 다양한 NPC와 상호작용하며 퀘스트를 수행하는 오픈월드 맵 전경</em></p></div>🛠 Tech StackCategoryTechnologyEngineUnity 2022.x (3D)LanguageC#ArchitectureManager Pattern, Data-Driven DesignDataScriptableObjectUITextMeshPro (TMP)🧩 System Architecture (UML)본 프로젝트는 데이터(Data)와 로직(Logic)의 분리를 핵심 설계 철학으로 삼고 있습니다.<div align="center"><img src="Images/digram.png" alt="Class Diagram" width="700"></div>🏗️ Design HighlightsScriptableObject 기반 데이터 설계: QuestBase를 상속받아 Kill, Collect, Reach 등 다양한 퀘스트 타입을 손쉽게 확장할 수 있습니다.중앙 집중식 관리 (Core Managers): * QuestManager: 모든 퀘스트의 상태 변경 및 이벤트(Kill/Collect) 처리를 총괄.DialogueManager: NPC와의 대화 흐름 제어 및 퀘스트 수락/완료 연결.느슨한 결합 (Loose Coupling): 몬스터나 아이템은 퀘스트 로직을 몰라도 되며, 이벤트 발생 시 매니저에게 알림만 보내는 구조로 의존성을 최소화했습니다.✨ Key Features & Implementation1. NPC 상호작용 및 퀘스트 수락 (Interaction)플레이어가 NPC에게 접근하여 대화를 나누고 퀘스트를 수락하는 과정입니다.Logic: DialogueManager가 NPC의 상태(수락 전/진행 중/완료)를 확인하여 적절한 대사를 출력하고 아이콘을 업데이트합니다.<div align="center"><img src="Images/퀘스트.jpg" alt="Quest Acceptance" width="45%" style="margin-right:10px;" /><img src="Images/image_a0b669.png" alt="Quest UI Update" width="45%" /></div>2. 다양한 미션 수행 (Gameplay Loop)QuestManager가 실시간으로 이벤트를 감지하여 목표 달성 여부를 체크합니다.Feature: 몬스터 처치(KillQuest), 특정 위치 도달(ReachQuest), 아이템 수집(CollectionQuest) 등 다양한 목표를 지원합니다.UI Feedback: 우측 상단 퀘스트 패널에 실시간 진행 상황(예: 처치 1/2)이 갱신됩니다.<div align="center"><img src="Images/미션진행.jpg" alt="Mission Progress" width="45%" style="margin-right:10px;" /><img src="Images/퀘스트2.jpg" alt="Reach Quest" width="45%" /></div>3. 퀘스트 완료 및 보상 (Completion)목표를 달성하고 NPC에게 돌아오면 완료 대사가 출력되며 퀘스트가 종료됩니다.Logic: 조건 충족 시(isCompleted = true) NPC 머리 위 아이콘 상태가 변경되고, 완료 대화 후 보상 로직이 실행됩니다.<div align="center"><img src="Images/퀘스트3.jpg" alt="Quest Return" width="45%" style="margin-right:10px;" /><img src="Images/퀘스트완료.jpg" alt="Quest Complete" width="45%" /></div>💻 Core Implementation Code확장 가능한 퀘스트 데이터 (QuestBase.cs)C#public enum QuestType { Kill, Reach, Collect }

// ScriptableObject를 상속받아 데이터 에셋으로 관리
public class QuestBase : ScriptableObject
{
    public string questName;
    public QuestType type;
    public bool isCompleted;
    public NPCInteraction ownerNPC;
}
중앙 퀘스트 관리자 (QuestManager.cs)C#public void MonsterKilled(string tag)
{
    foreach (var quest in currentQuests)
    {
        // 타입이 Kill이고, 태그가 일치하며, 미완료 상태인 경우
        if (quest.type == QuestType.Kill && !quest.isCompleted)
        {
            KillQuest killQ = (KillQuest)quest;
            if (killQ.targetTag == tag)
            {
                killQ.currentKill++;
                UpdateQuestUI(); // UI 실시간 갱신

                // 목표 달성 시 아이콘 변경 (완료 가능 상태 알림)
                if (killQ.currentKill >= killQ.killAmount)
                {
                    if (quest.ownerNPC != null)
                        quest.ownerNPC.questIcon.UpdateIcon(QuestIconState.Complete);
                }
            }
        }
    }
}
Developer: [본인 이름/닉네임]Contact: [이메일 주소]
