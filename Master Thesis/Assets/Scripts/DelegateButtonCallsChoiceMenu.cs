using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DelegateButtonCallsChoiceMenu : MonoBehaviour
{
    public Button button0R, button45R, button90R, button135R, button180R, button225R, button270R, button315R;
    public Button button0B, button45B, button90B, button135B, button180B, button225B, button270B, button315B;

    private Button buttonRActive;
    private Button buttonBActive;
    private Canvas canvas;
    private float rotationZRed, rotationZBlue;
    private bool choiceMade = false;

    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }

    public void button0Red()
    {
        buttonRActive = button0R;
        selectButton(button0R);
    }

    public void button45Red()
    {
        buttonRActive = button45R;
        selectButton(button45R);
    }

    public void button90Red()
    {
        buttonRActive = button90R;
        selectButton(button90R);
    }

    public void button135Red()
    {
        buttonRActive = button135R;
        selectButton(button135R);
    }

    public void button180Red()
    {
        buttonRActive = button180R;
        selectButton(button180R);
    }

    public void button225Red()
    {
        buttonRActive = button225R;
        selectButton(button225R);
    }

    public void button270Red()
    {
        buttonRActive = button270R;
        selectButton(button270R);
    }

    public void button315Red()
    {
        buttonRActive = button315R;
        selectButton(button315R);
    }



    public void button0Blue()
    {
        buttonBActive = button0B;
        selectButton(button0B);
    }

    public void button45Blue()
    {
        buttonBActive = button45B;
        selectButton(button45B);
    }

    public void button90Blue()
    {
        buttonBActive = button90B;
        selectButton(button90B);
    }

    public void button135Blue()
    {
        buttonBActive = button135B;
        selectButton(button135B);
    }

    public void button180Blue()
    {
        buttonBActive = button180B;
        selectButton(button180B);
    }

    public void button225Blue()
    {
        buttonBActive = button225B;
        selectButton(button225B);
    }

    public void button270Blue()
    {
        buttonBActive = button270B;
        selectButton(button270B);
    }

    public void button315Blue()
    {
        buttonBActive = button315B;
        selectButton(button315B);
    }

    public void Reset()
    {
        choiceMade = false;
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
        rotationZBlue = n;
    }

    public void toggleUI()
    {
        canvas.enabled = !canvas.enabled;
    }

    private void selectButton(Button b)
    {
        selectButtonWithColor(b, Color.yellow);
    }

    private void selectButtonWithColor(Button b, Color c)
    {
        var colors = b.colors;
        colors.normalColor = c;
        b.colors = colors;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showResults()
    {
        if (buttonRActive == button0R && rotationZRed == 0) selectButtonWithColor(buttonRActive, Color.green);
        else if (buttonRActive == button45R && rotationZRed == 45) selectButtonWithColor(buttonRActive, Color.green);
        else if (buttonRActive == button90R && rotationZRed == 90) selectButtonWithColor(buttonRActive, Color.green);
        else if (buttonRActive == button135R && rotationZRed == 135) selectButtonWithColor(buttonRActive, Color.green);
        else if (buttonRActive == button180R && rotationZRed == 180) selectButtonWithColor(buttonRActive, Color.green);
        else if (buttonRActive == button225R && rotationZRed == 225) selectButtonWithColor(buttonRActive, Color.green);
        else if (buttonRActive == button270R && rotationZRed == 270) selectButtonWithColor(buttonRActive, Color.green);
        else if (buttonRActive == button315R && rotationZRed == 315) selectButtonWithColor(buttonRActive, Color.green);
        else
        {
            selectButtonWithColor(buttonRActive, Color.red);
            selectCorrectButtonRGreen();
        }

        if (buttonBActive == button0B && rotationZBlue == 0) selectButtonWithColor(buttonBActive, Color.green);
        else if (buttonBActive == button45B && rotationZBlue == 45) selectButtonWithColor(buttonBActive, Color.green);
        else if (buttonBActive == button90B && rotationZBlue == 90) selectButtonWithColor(buttonBActive, Color.green);
        else if (buttonBActive == button135B && rotationZBlue == 135) selectButtonWithColor(buttonBActive, Color.green);
        else if (buttonBActive == button180B && rotationZBlue == 180) selectButtonWithColor(buttonBActive, Color.green);
        else if (buttonBActive == button225B && rotationZBlue == 225) selectButtonWithColor(buttonBActive, Color.green);
        else if (buttonBActive == button270B && rotationZBlue == 270) selectButtonWithColor(buttonBActive, Color.green);
        else if (buttonBActive == button315B && rotationZBlue == 315) selectButtonWithColor(buttonBActive, Color.green);
        else
        {
            selectButtonWithColor(buttonBActive, Color.red);
            selectCorrectButtonBGreen();
        }

        choiceMade = true;
    }

    private void selectCorrectButtonRGreen()
    {
        if (rotationZRed == 0) selectButtonWithColor(button0R, Color.green);
        if (rotationZRed == 45) selectButtonWithColor(button45R, Color.green);
        if (rotationZRed == 90) selectButtonWithColor(button90R, Color.green);
        if (rotationZRed == 135) selectButtonWithColor(button135R, Color.green);
        if (rotationZRed == 180) selectButtonWithColor(button180R, Color.green);
        if (rotationZRed == 225) selectButtonWithColor(button225R, Color.green);
        if (rotationZRed == 270) selectButtonWithColor(button270R, Color.green);
        if (rotationZRed == 315) selectButtonWithColor(button315R, Color.green);
    }

    private void selectCorrectButtonBGreen()
    {
        if (rotationZBlue == 0) selectButtonWithColor(button0B, Color.green);
        if (rotationZBlue == 45) selectButtonWithColor(button45B, Color.green);
        if (rotationZBlue == 90) selectButtonWithColor(button90B, Color.green);
        if (rotationZBlue == 135) selectButtonWithColor(button135B, Color.green);
        if (rotationZBlue == 180) selectButtonWithColor(button180B, Color.green);
        if (rotationZBlue == 225) selectButtonWithColor(button225B, Color.green);
        if (rotationZBlue == 270) selectButtonWithColor(button270B, Color.green);
        if (rotationZBlue == 315) selectButtonWithColor(button315B, Color.green);
    }
}
