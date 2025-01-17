using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Localization.Settings;
public class LocaleManager : MonoBehaviour
{
    ///<summary>
    ///Esta clase se encarga de administrar todos los locales de las traducciones
    ///</summary>
    private bool active = false;
    private int maxLocalesCount = 1;
    public int localeIndex = 1;

    public void ChangeLocaleUp(){
        if(active == true)
            return;
        if(localeIndex == maxLocalesCount){ localeIndex = 0;}else{ localeIndex++;}
        StartCoroutine(SetLocale(localeIndex));
    }
    public void ChangeLocaleDown(){
        if(active == true)
            return;
        if(localeIndex == 0){ localeIndex = maxLocalesCount;}else{ localeIndex--;}
        StartCoroutine(SetLocale(localeIndex));
    }

    IEnumerator SetLocale(int localeID){
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        active = false;
    }
}