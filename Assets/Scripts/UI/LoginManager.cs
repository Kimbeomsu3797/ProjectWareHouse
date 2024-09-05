using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField id;

    public TMP_InputField password;

    public TextMeshProUGUI notify;
    // Start is called before the first frame update
    void Start()
    {
        //검사 텍스트 창을 비운다.
        notify.text = "";
    }

    // Update is called once per frame
    void Update()
    {

    }
    //아이디와 패스워드 저장 함수
    public void SaveUserData()
    {
        // 만일 입력 검사에 문제가 있으면 함수를 종료한다.
        if (!CheckInput(id.text, password.text))
        {
            return;
        }

        //만일 시스템에 저장되어 있는 아이디가 존재하지 않는다면...
        if (!PlayerPrefs.HasKey(id.text))
        {
            //사용자의 아이디는 키로 패스워드를 값으로 설정하여 저장한다.
            //검사 텍스트에 아이디생성이 완료되었습니다. 출력
            PlayerPrefs.SetString(id.text, password.text);
            notify.text = "아이디 생성이 완료되었습니다";
        }
        //그렇지 않으면 이미 존재한다는 메세지를 출력한다.
        else
        {
            //검사 텍스트에 이미 존재하는 아이디입니다. 출력
            notify.text = "이미 존재하는 아이디 입니다.";
        }
    }
    bool CheckInput(string id, string pwd)
    {
        //만일, 아이디와 패스워드 입력란이 하나라도 비어있으면 유저 정보 입력을 요구한다.
        if (id == "" || pwd == "")
        {
            notify.text = "아이디 또는 패스워드를 입력해주세요.";
            //검사 텍스트에 "아이디 또는 패스워드를 입력해주세요." 출력
            return false;
        }
        //입력이 비어있지 않으면 true를 반환한다.
        else
        {
            return true;
        }
    }
    //로그인 함수
    public void CheckUseData()
    {
        //만일 입력 검사에 문제가 있으면 함수를 종료한다.
        if (!CheckInput(id.text, password.text))
        {
            return;
        }

        //사용자가 입력한 아이디를 키로 사용해서 시스템에 저장된 값을 불러온다.
        string pass = PlayerPrefs.GetString(id.text);
        //만일, 사용자가 입력한 패스워드와 시스템에서 불러온 값을 비교해서 동일하다면
        if (password.text == pass)
        {
            //다음 씬(1번 씬 : firstScene(나의 경우엔 mainscene)을 로드한다.
            SceneManager.LoadScene(1);
            PlayerPrefs.SetInt("loadSceneNum", 2);
        }
        //그렇지 않고 두 데이터의 값이 다르다면, 유저 정보 불일치 메세지를 남긴다.
        else
        {
            //검사 텍스트에 "입력하신 아이디와 패스워드가 일치하지 않습니다." 출력
            notify.text = "입력하신 아이디와 패스워드가 일치하지 않습니다.";
        }
    }
}
