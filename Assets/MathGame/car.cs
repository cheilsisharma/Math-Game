using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class car : MonoBehaviour
{
    public GameObject box;
    public TMP_Text question_tampil;
    public TMP_Text score_tampil;
    public Button restartButton;
    public Button quitButton;
    public GameObject gameOverUI; // Reference to the Game Over UI GameObject
    public float speed;
    public int minOperand = 1;
    public int maxOperand = 20;
    public int maxScore = 150;
    public int fixedScore = 20; // Fixed score
    public int scoreDeductionOnWrongAnswer = 2;

    // Add the mathematical operators (+, -, *, /) in the Inspector
    public string[] operators = { "+", "-", "*", "/" };

    private int operand1;
    private int operand2;
    private string selectedOperator;
    private int correctAnswer;
    private int score;
    private bool gameOver = false;
    private bool isWaitingForAnswer = false;

    private List<GameObject> cubes;
    private List<int> usedCubeIndices;

    private void Start()
    {
        score = fixedScore; // Initialize the score to the fixed score
        UpdateScoreDisplay();

        cubes = new List<GameObject>();
        usedCubeIndices = new List<int>();

        for (int i = 0; i < box.transform.childCount; i++)
        {
            cubes.Add(box.transform.GetChild(i).gameObject);
        }

        StartCoroutine(LanjutQuestion());
        restartButton.onClick.AddListener(restart);
        quitButton.onClick.AddListener(QuitGame);

        // Hide the Game Over UI at the start
        gameOverUI.SetActive(false);
    }

    private void UpdateScoreDisplay()
    {
        score_tampil.text = score.ToString();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(-speed, 0, 0);
            transform.rotation = Quaternion.Euler(0, -10, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(speed, 0, 0);
            transform.rotation = Quaternion.Euler(0, 10, 0);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }


    private void OnTriggerEnter(Collider obj)
    {
        if (!gameOver && obj.name == "box" && isWaitingForAnswer)
        {
            TMP_Text answerText = obj.transform.GetChild(0).GetComponent<TMP_Text>();

            int userAnswer;
            if (int.TryParse(answerText.text, out userAnswer))
            {
                if (userAnswer == correctAnswer)
                {
                    // Correct answer, no score change
                    obj.gameObject.SetActive(false);

                    if (score >= maxScore)
                    {
                        Congratulations();
                        return;
                    }
                }
                else
                {
                    // Wrong answer, deduct points
                    score -= scoreDeductionOnWrongAnswer;
                    UpdateScoreDisplay();

                    if (score <= 0)
                    {
                        score = 0; // Ensure score doesn't go below 0
                        GameOver();
                        return;
                    }
                }

                // Change the question and reactivate the cubes
                StartCoroutine(LanjutQuestion());
            }
        }
    }

    private IEnumerator LanjutQuestion()
    {
        yield return new WaitForSeconds(1.5f);

        // Reactivate all cubes except those already used for questions
        foreach (var cube in cubes)
        {
            cube.SetActive(true);
        }

        usedCubeIndices.Clear();

        // Randomly select operands and operator
        operand1 = UnityEngine.Random.Range(minOperand, maxOperand + 1);
        operand2 = UnityEngine.Random.Range(minOperand, operand1); // Ensure denominator is less than numerator
        selectedOperator = operators[UnityEngine.Random.Range(0, operators.Length)];

        // Calculate the correct answer based on the selected operator
        switch (selectedOperator)
        {
            case "+":
                correctAnswer = operand1 + operand2;
                break;
            case "-":
                correctAnswer = operand1 - operand2;
                break;
            case "*":
                correctAnswer = operand1 * operand2;
                break;
            case "/":
                // Calculate operands and ensure a non-zero division
                operand2 = Mathf.Max(1, operand2);
                correctAnswer = operand1 / operand2;
                break;
        }

        // Display the question
        question_tampil.text = $"{operand1} {selectedOperator} {operand2} = ?";
        isWaitingForAnswer = true;

        // Randomly select which cube will display the correct answer (0, 1, or 2)
        int correctAnswerCubeIndex = UnityEngine.Random.Range(0, cubes.Count);
        usedCubeIndices.Add(correctAnswerCubeIndex);

        // Assign the correct answer to the selected cube
        TMP_Text correctAnswerText = cubes[correctAnswerCubeIndex].transform.GetChild(0).GetComponent<TMP_Text>();
        correctAnswerText.text = correctAnswer.ToString();

        // Assign random incorrect answers to the remaining cubes
        for (int i = 0; i < cubes.Count; i++)
        {
            if (i != correctAnswerCubeIndex)
            {
                TMP_Text answerText = cubes[i].transform.GetChild(0).GetComponent<TMP_Text>();
                int incorrectAnswer;
                do
                {
                    incorrectAnswer = UnityEngine.Random.Range(minOperand, maxOperand + 1);
                } while (incorrectAnswer == correctAnswer);
                answerText.text = incorrectAnswer.ToString();
            }
        }
    }

    private void Congratulations()
    {
        // Handle game completion logic here
    }

    private void GameOver()
    {
        gameOver = true;
        // Display the Game Over UI
        gameOverUI.SetActive(true);
    }

    public void restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Main1");
    }
}
