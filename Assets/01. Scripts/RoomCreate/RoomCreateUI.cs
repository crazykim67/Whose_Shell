using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class CreateGameRoomData
{
    public int terrapinCount;
    public int maxPlayerCount;
}

public class RoomCreateUI : MonoBehaviour
{
    public GameObject roomCreateUI;

    [Header("MainMenu UI")]
    public MainMenuUI mainMenuUI;

    public TextMeshProUGUI errorText;
    public TMP_InputField nickNameInputField;

    public Animator nickAnim;

    private float timer;
    private bool isError;

    public List<Image> crewImages;

    public List<Button> terrapinCountButtons;

    public List<Button> maxPlayerCountButtons;

    private CreateGameRoomData roomData;

    private void Start()
    {
        for(int i = 0;i < crewImages.Count; i++)
        {
            Material mat = Instantiate(crewImages[i].material);
            crewImages[i].material = mat;
        }

        roomData = new CreateGameRoomData() { terrapinCount = 1, maxPlayerCount = 10};
        UpdateCrewImage();
    }

    private void Update()
    {
        if (isError)
        {
            if(timer < 3f)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                isError = false;
                errorText.enabled = false;
            }
        }
    }

    #region Show & Hide

    public void OnShow()
    {
        roomCreateUI.SetActive(true);
        Init();
    }

    public void OnHide()
    {
        roomCreateUI.SetActive(false);
    }

    #endregion

    public void Init()
    {
        roomData = new CreateGameRoomData() { terrapinCount = 1, maxPlayerCount = 10 };
        UpdateCrewImage();
    }

    #region Mirror 관련

    public void OnCreateRoom()
    {
        if (nickNameInputField.text.Equals(string.Empty))
        {
            OnNickAnim();
            OnError("닉네임 입력 칸이 비어있습니다.");
            return;
        }
    }

    #endregion

    // 플레이어 인원 설정
    public void UpdateMaxPlayerCount(int count)
    {
        roomData.maxPlayerCount = count;

        for (int i = 0; i < maxPlayerCountButtons.Count; i++)
        {
            if (i == count - 4)
                maxPlayerCountButtons[i].image.color = new Color(1f, 1f, 1f, 1f);
            else
                maxPlayerCountButtons[i].image.color = new Color(1f, 1f, 1f, 0f);
        }

        UpdateCrewImage();
    }

    // 자라 수 정하는 메서드
    public void UpdateTerrapinCount(int count)
    {
        roomData.terrapinCount = count;

        for(int i = 0; i < terrapinCountButtons.Count; i++)
        {
            if(i == count - 1)
                terrapinCountButtons[i].image.color = new Color(1f, 1f, 1f, 1f);
            else
                terrapinCountButtons[i].image.color = new Color(1f, 1f, 1f, 0f);
        }

        int limitMaxPlayer = count == 1 ? 4 : count == 2 ? 7 : 9;

        if (roomData.maxPlayerCount < limitMaxPlayer)
            UpdateMaxPlayerCount(limitMaxPlayer);
        else
            UpdateMaxPlayerCount(roomData.maxPlayerCount);

        for(int i = 0; i < maxPlayerCountButtons.Count; i++)
        {
            var text = maxPlayerCountButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if(i < limitMaxPlayer - 4)
            {
                maxPlayerCountButtons[i].interactable = false;
                text.color = Color.gray;
            }
            else
            {
                maxPlayerCountButtons[i].interactable = true;
                text.color = Color.white;
            }
        }

        UpdateCrewImage();
    }

    // 업데이트 이미지
    private void UpdateCrewImage()
    {
        int terrapinCount = roomData.terrapinCount;
        int index = 0;

        for(int i = 0;i < crewImages.Count; i++)
            crewImages[i].material.SetFloat("_Hue", 0f);

        while(terrapinCount != 0)
        {
            if(index >= roomData.maxPlayerCount)
                index = 0;

            if (crewImages[index].material.GetFloat("_Hue") != 290f && Random.Range(0, 5) == 0)
            {
                crewImages[index].material.SetFloat("_Hue", 290f);
                terrapinCount--;
            }
            index++;
        }

        for(int i = 0; i < crewImages.Count; i++)
        {
            if(i < roomData.maxPlayerCount)
                crewImages[i].gameObject.SetActive(true);
            else
                crewImages[i].gameObject.SetActive(false);
        }
    }

    // 뒤로가기
    public void OnCancel()
    {
        CircleTransitionController.Instance.CloseBlackScreen(OnHide, mainMenuUI.OnShow);
    }

    public void OnError(string str)
    {
        errorText.enabled = true;
        errorText.text = str;
        isError = true;
        timer = 0;
    }

    public void OnNickAnim()
    {
        nickAnim.SetTrigger("Error");
    }
}
