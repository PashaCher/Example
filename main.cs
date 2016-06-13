using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
 
public class main : MonoBehaviour
{
    public Canvas CanvasObject;

    private Vector3 nextPosition = Vector3.zero;
    public bool isFinish = false;
    public Frame lockedFrame = null;
    public bool canMove = false;
    private int moveCount = 0;
    public GameObject gameObjects;

    public int record;
    public int score;

    /*[Header("Название след. уровня")]
    public string nextlevelname;*/
    [Header("Объект текста")]
    public GameObject CanvObject;

    void Start()
    {
        string name = SceneManager.GetActiveScene().name;
        record = PlayerPrefs.GetInt("record_" + name, 0);
        CanvasObject.enabled = false;
    }

    void OnGUI()
    {
        GUI.color = Color.black;
        GUI.Label(new Rect(10, 10, 100, 100), "Movies: " + score);

        GUI.color = Color.black;
        GUI.Label(new Rect(10, 25, 100, 100), "Record: " + record);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            nextPosition = Vector3.up * 3;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            nextPosition = -Vector3.up * 3;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            nextPosition = Vector3.left * 3;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            nextPosition = -Vector3.left * 3;
        }

        var newPos = this.transform.position + nextPosition;
        if (SceneManager.GetActiveScene().name == "level1")
        {
            newPos.x = Mathf.Clamp(newPos.x, -3, 3);
            newPos.y = Mathf.Clamp(newPos.y, -3, 3);
        }

        if (SceneManager.GetActiveScene().name == "level2")
        {
            newPos.x = Mathf.Clamp(newPos.x, -3, 3);
            newPos.y = Mathf.Clamp(newPos.y, -3, 3);
        }

        if (SceneManager.GetActiveScene().name == "level3")
        {
            newPos.x = Mathf.Clamp(newPos.x, -3, 3);
            newPos.y = Mathf.Clamp(newPos.y, -3, 3);
        }

        RaycastHit2D hit = Physics2D.CircleCast(newPos, 0.1f, Vector3.zero);

        bool canMove = !isFinish;

        if (hit.collider != null)
        {
            Frame frame = hit.collider.GetComponent<Frame>();
            canMove = CanMove(frame, nextPosition);

            if (canMove)
            {
                if (lockedFrame == null)
                {
                    lockedFrame = frame;
                }
                else
                {
                    if (lockedFrame.size < frame.size)
                    {
                        isFinish = true;
                    }
                    else
                    {
                        canMove = false;
                    }
                }
            }
        }

        if (canMove && nextPosition.magnitude > 0)
        {
            if (lockedFrame != null && !CanMove(lockedFrame, -nextPosition))
            {
                lockedFrame.transform.position = newPos;
            }
            else
            {
                lockedFrame = null;
            }
            moveCount++;
            score++;

            this.transform.position = newPos;
        }


        nextPosition = Vector3.zero;

        if (isFinish)
        {
            StartCoroutine(FinishGame());
        }
    }

    private bool CanMove(Frame frame, Vector3 d)
    {
        bool canMove = true;
        Vector3 dir = d;
        if (frame.type != Frame.Types.Up && dir.y < 0)
        {
            canMove = false;
        }

        if (frame.type != Frame.Types.Down && dir.y > 0)
        {
            canMove = false;
        }

        if (frame.type != Frame.Types.Left && dir.x > 0)
        {
            canMove = false;
        }

        if (frame.type != Frame.Types.Right && dir.x < 0)
        {
            canMove = false;
        }

        return canMove;
    }

    IEnumerator FinishGame()
    {
        CanvasObject.enabled = true;
        CanvObject.SetActive(true);
        CanvObject.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(1);


        string name = SceneManager.GetActiveScene().name;
        if (score != 0 && (score < record || record == 0))
        {
            PlayerPrefs.SetInt("record_" + name, score);
            record = score;
            PlayerPrefs.Save();
        }
        Debug.Log(record.ToString());

        CanvObject.GetComponent<Animator>().enabled = false;
		SceneManager.LoadScene("FinishLevel");
    }
}
