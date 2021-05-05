using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DelegateButtonCallsChoiceMenu : MonoBehaviour
{
    public Button button0R, button45R, button90R, button135R, button180R, button225R, button270R, button315R;
    public Button button0B, button45B, button90B, button135B, button180B, button225B, button270B, button315B;
    public Canvas canvas;
    public AnticipitationScript anticipationScript;
    public SphereToSpawnGreyCube sphere;
    public CanvasDisablerOnStart cdos;

    private Button buttonRActive;
    private Button buttonBActive;
    
    private float rotationZRed, rotationZBlue;
    private bool choiceMade = false;
    private int scoreRightChoices = 0;
    private float rotationZRedChoosen, rotationZBlueChoosen;

    void Start()
    {
        canvas.enabled = false;
    }

    public int getScoreRightChoices()
    {
        return scoreRightChoices;
    }

    public float getRotationChoosenRed()
    {
        return rotationZRedChoosen;
    }

    public float getRotationChoosenBlue()
    {
        return rotationZBlueChoosen;
    }

    public void startAntizipationstest()
    {
        cdos.resumeGame();
        cdos.disableCanvas();
        sphere.deactivatePointer(false);
        sphere.deactivatePointer(true);
    }

    public void button0Red()
    {
        buttonRActive = button0R;
        rotationZRedChoosen = 0;
        resetAllRedButtons();
        selectButtonR(button0R);
    }

    public void button45Red()
    {
        buttonRActive = button45R;
        rotationZRedChoosen = 45;
        resetAllRedButtons();
        selectButtonR(button45R);
    }

    public void button90Red()
    {
        buttonRActive = button90R;
        rotationZRedChoosen = 90;
        resetAllRedButtons();
        selectButtonR(button90R);
    }

    public void button135Red()
    {
        buttonRActive = button135R;
        rotationZRedChoosen = 135;
        resetAllRedButtons();
        selectButtonR(button135R);
    }

    public void button180Red()
    {
        buttonRActive = button180R;
        rotationZRedChoosen = 180;
        resetAllRedButtons();
        selectButtonR(button180R);
    }

    public void button225Red()
    {
        buttonRActive = button225R;
        rotationZRedChoosen = 225;
        resetAllRedButtons();
        selectButtonR(button225R);
    }

    public void button270Red()
    {
        buttonRActive = button270R;
        rotationZRedChoosen = 270;
        resetAllRedButtons();
        selectButtonR(button270R);
    }

    public void button315Red()
    {
        buttonRActive = button315R;
        rotationZRedChoosen = 315;
        resetAllRedButtons();
        selectButtonR(button315R);
    }



    public void button0Blue()
    {
        buttonBActive = button0B;
        rotationZBlueChoosen = 0;
        resetAllBlueButtons();
        selectButtonB(button0B);
    }

    public void button45Blue()
    {
        buttonBActive = button45B;
        rotationZBlueChoosen = 45;
        resetAllBlueButtons();
        selectButtonB(button45B);
    }

    public void button90Blue()
    {
        buttonBActive = button90B;
        rotationZBlueChoosen = 90;
        resetAllBlueButtons();
        selectButtonB(button90B);
    }

    public void button135Blue()
    {
        buttonBActive = button135B;
        rotationZBlueChoosen = 135;
        resetAllBlueButtons();
        selectButtonB(button135B);
    }

    public void button180Blue()
    {
        buttonBActive = button180B;
        rotationZBlueChoosen = 180;
        resetAllBlueButtons();
        selectButtonB(button180B);
    }

    public void button225Blue()
    {
        buttonBActive = button225B;
        rotationZBlueChoosen = 225;
        resetAllBlueButtons();
        selectButtonB(button225B);
    }

    public void button270Blue()
    {
        buttonBActive = button270B;
        rotationZBlueChoosen = 270;
        resetAllBlueButtons();
        selectButtonB(button270B);
    }

    public void button315Blue()
    {
        buttonBActive = button315B;
        rotationZBlueChoosen = 315;
        resetAllBlueButtons();
        selectButtonB(button315B);
    }

    public void Reset()
    {
        buttonRActive = null;
        buttonBActive = null;
        choiceMade = false;
        resetAllRedButtons();
        resetAllBlueButtons();
    }

    private void resetAllRedButtons()
    {
        selectButtonWithColor(button0R, Color.white);
        selectButtonWithColor(button45R, Color.white);
        selectButtonWithColor(button90R, Color.white);
        selectButtonWithColor(button135R, Color.white);
        selectButtonWithColor(button180R, Color.white);
        selectButtonWithColor(button225R, Color.white);
        selectButtonWithColor(button270R, Color.white);
        selectButtonWithColor(button315R, Color.white);
    }

    private void resetAllBlueButtons()
    {
        selectButtonWithColor(button0B, Color.white);
        selectButtonWithColor(button45B, Color.white);
        selectButtonWithColor(button90B, Color.white);
        selectButtonWithColor(button135B, Color.white);
        selectButtonWithColor(button180B, Color.white);
        selectButtonWithColor(button225B, Color.white);
        selectButtonWithColor(button270B, Color.white);
        selectButtonWithColor(button315B, Color.white);
    }

    public bool choiceWasMade()
    {
        return choiceMade;
    }

    public void setRotationZRed(float n)
    {
        rotationZRed = n;
    }

    public void setRotationZBlue(float n)
    {
        Debug.Log("RotationZ Blue: " + n);
        rotationZBlue = n;
    }

    public void toggleUI()
    {
        canvas.enabled = !canvas.enabled;
    }

    private void selectButtonB(Button b)
    {
        selectButtonWithColor(b, new Color(0 / 255.0f, 0 / 255.0f, 139 / 255.0f));
    }

    private void selectButtonR(Button b)
    {
        selectButtonWithColor(b, new Color(139 / 255.0f, 0 / 255.0f, 0 / 255.0f));
    }

    private void selectButtonWithColor(Button b, Color c)
    {
        if (b == null) return;
        var colors = b.colors;
        colors.normalColor = c;
        colors.highlightedColor = c;
        b.colors = colors;
    }

    private void selectButtonWithColorAndScoreOnePoint(Button b, Color c)
    {
        scoreOnePoint();
        selectButtonWithColor(b, c);
    }

    private void scoreOnePoint()
    {
        scoreRightChoices += 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showResults()
    {
        if (buttonRActive == null || buttonBActive == null) return;

        if (anticipationScript.showResultsChoiceMenu)
        {
            if (buttonRActive == button0R && rotationZRed == 0) selectButtonWithColorAndScoreOnePoint(buttonRActive, Color.green);
            else if (buttonRActive == button45R && rotationZRed == 45) selectButtonWithColorAndScoreOnePoint(buttonRActive, Color.green);
            else if (buttonRActive == button90R && rotationZRed == 90) selectButtonWithColorAndScoreOnePoint(buttonRActive, Color.green);
            else if (buttonRActive == button135R && rotationZRed == 135) selectButtonWithColorAndScoreOnePoint(buttonRActive, Color.green);
            else if (buttonRActive == button180R && rotationZRed == 180) selectButtonWithColorAndScoreOnePoint(buttonRActive, Color.green);
            else if (buttonRActive == button225R && rotationZRed == 225) selectButtonWithColorAndScoreOnePoint(buttonRActive, Color.green);
            else if (buttonRActive == button270R && rotationZRed == 270) selectButtonWithColorAndScoreOnePoint(buttonRActive, Color.green);
            else if (buttonRActive == button315R && rotationZRed == 315) selectButtonWithColorAndScoreOnePoint(buttonRActive, Color.green);
            else
            {
                selectButtonWithColor(buttonRActive, new Color(136 / 255.0f, 0 / 255.0f, 255 / 255.0f));
                selectCorrectButtonRGreen();
            }

            if (buttonBActive == button0B && rotationZBlue == 0) selectButtonWithColorAndScoreOnePoint(buttonBActive, Color.green);
            else if (buttonBActive == button45B && rotationZBlue == 45) selectButtonWithColorAndScoreOnePoint(buttonBActive, Color.green);
            else if (buttonBActive == button90B && rotationZBlue == 90) selectButtonWithColorAndScoreOnePoint(buttonBActive, Color.green);
            else if (buttonBActive == button135B && rotationZBlue == 135) selectButtonWithColorAndScoreOnePoint(buttonBActive, Color.green);
            else if (buttonBActive == button180B && rotationZBlue == 180) selectButtonWithColorAndScoreOnePoint(buttonBActive, Color.green);
            else if (buttonBActive == button225B && rotationZBlue == 225) selectButtonWithColorAndScoreOnePoint(buttonBActive, Color.green);
            else if (buttonBActive == button270B && rotationZBlue == 270) selectButtonWithColorAndScoreOnePoint(buttonBActive, Color.green);
            else if (buttonBActive == button315B && rotationZBlue == 315) selectButtonWithColorAndScoreOnePoint(buttonBActive, Color.green);
            else
            {
                selectButtonWithColor(buttonBActive, new Color(136 / 255.0f, 0 / 255.0f, 255 / 255.0f));
                selectCorrectButtonBGreen();
            }
        }
        else
        {
            if (buttonRActive == button0R && rotationZRed == 0) scoreOnePoint();
            else if (buttonRActive == button45R && rotationZRed == 45) scoreOnePoint();
            else if (buttonRActive == button90R && rotationZRed == 90) scoreOnePoint();
            else if (buttonRActive == button135R && rotationZRed == 135) scoreOnePoint();
            else if (buttonRActive == button180R && rotationZRed == 180) scoreOnePoint();
            else if (buttonRActive == button225R && rotationZRed == 225) scoreOnePoint();
            else if (buttonRActive == button270R && rotationZRed == 270) scoreOnePoint();
            else if (buttonRActive == button315R && rotationZRed == 315) scoreOnePoint();

            if (buttonBActive == button0B && rotationZBlue == 0) scoreOnePoint();
            else if (buttonBActive == button45B && rotationZBlue == 45) scoreOnePoint();
            else if (buttonBActive == button90B && rotationZBlue == 90) scoreOnePoint();
            else if (buttonBActive == button135B && rotationZBlue == 135) scoreOnePoint();
            else if (buttonBActive == button180B && rotationZBlue == 180) scoreOnePoint();
            else if (buttonBActive == button225B && rotationZBlue == 225) scoreOnePoint();
            else if (buttonBActive == button270B && rotationZBlue == 270) scoreOnePoint();
            else if (buttonBActive == button315B && rotationZBlue == 315) scoreOnePoint();
        }

        choiceMade = true;
       
    }

    private void selectCorrectButtonRGreen()
    {
        int rotZRed = Mathf.RoundToInt(rotationZRed);
        if (rotZRed == 0 || rotZRed == -360 || rotZRed == 360) selectButtonWithColor(button0R, Color.green);
        if (rotZRed == 45 || rotZRed == -315) selectButtonWithColor(button45R, Color.green);
        if (rotZRed == 90 || rotZRed == -270) selectButtonWithColor(button90R, Color.green);
        if (rotZRed == 135 || rotZRed == -225) selectButtonWithColor(button135R, Color.green);
        if (rotZRed == 180 || rotZRed == -180) selectButtonWithColor(button180R, Color.green);
        if (rotZRed == 225 || rotZRed == -135) selectButtonWithColor(button225R, Color.green);
        if (rotZRed == 270 || rotZRed == -90) selectButtonWithColor(button270R, Color.green);
        if (rotZRed == 315 || rotZRed == -45) selectButtonWithColor(button315R, Color.green);
    }

    private void selectCorrectButtonBGreen()
    {
        int rotZBlue = Mathf.RoundToInt(rotationZBlue);
        if (rotZBlue == 0 || rotZBlue == -360 || rotZBlue == 360) selectButtonWithColor(button0B, Color.green);
        if (rotZBlue == 45 || rotZBlue == -315) selectButtonWithColor(button45B, Color.green);
        if (rotZBlue == 90 || rotZBlue == -270) selectButtonWithColor(button90B, Color.green);
        if (rotZBlue == 135 || rotZBlue == -225) selectButtonWithColor(button135B, Color.green);
        if (rotZBlue == 180 || rotZBlue == -180) selectButtonWithColor(button180B, Color.green);
        if (rotZBlue == 225 || rotZBlue == -135) selectButtonWithColor(button225B, Color.green);
        if (rotZBlue == 270 || rotZBlue == -90) selectButtonWithColor(button270B, Color.green);
        if (rotZBlue == 315 || rotZBlue == -45) selectButtonWithColor(button315B, Color.green);
    }
}
