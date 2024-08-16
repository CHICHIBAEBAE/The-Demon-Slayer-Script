using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ResolutionManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown fullscreenDropdown;

    private Resolution[] resolutions;
    private Resolution initialResolution = new Resolution { width = 1440, height = 900 };
    private bool initialFullscreenMode = false; 

    void Start()
    {
        // ��� �ػ� ��������
        resolutions = Screen.resolutions;

        // �ߺ� �ػ� ����
        resolutions = resolutions.Distinct(new ResolutionComparer()).ToArray();

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == initialResolution.width &&
                resolutions[i].height == initialResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // �ػ� ���� �̺�Ʈ ������ �߰�
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        fullscreenDropdown.ClearOptions();
        List<string> fullscreenOptions = new List<string> { "Fullscreen", "Windowed" };
        fullscreenDropdown.AddOptions(fullscreenOptions);
        fullscreenDropdown.value = initialFullscreenMode ? 0 : 1; // Windowed mode is selected by default
        fullscreenDropdown.RefreshShownValue();

        // ��ü ȭ�� ��� ���� �̺�Ʈ ������ �߰�
        fullscreenDropdown.onValueChanged.AddListener(SetFullscreenMode);

        // �ʱ� ���� ����
        SetResolution(currentResolutionIndex);
        SetFullscreenMode(fullscreenDropdown.value);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreenMode(int fullscreenIndex)
    {
        bool isFullscreen = fullscreenIndex == 0; // 0 = Fullscreen, 1 = Windowed

        // ���� ��ü ȭ�� ���� ���� ������ ��尡 �ٸ� ���� ����
        if (Screen.fullScreen != isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }
    }
}

// �ػ󵵸� ���Ͽ� �ߺ� �����ϴ� Comparer Ŭ����
public class ResolutionComparer : IEqualityComparer<Resolution>
{
    public bool Equals(Resolution x, Resolution y)
    {
        return x.width == y.width && x.height == y.height;
    }

    public int GetHashCode(Resolution obj)
    {
        return obj.width.GetHashCode() ^ obj.height.GetHashCode();
    }
}
