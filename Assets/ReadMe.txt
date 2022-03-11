Was zu beachten ist:
Sky and Fog Volume: Ich habe die Sky and Fog Volume leicht angepasst, sodass sie die Voraussetzungen für die Varjo SDK erfüllen  (Fokus-Rechteck sollte dadurch verschwunden sein). Wenn die Helligkeit nicht passt - einfach unter Exposure anpassen.

DeviceManager: Dieses Objekt ist dazu gedacht, zwischen Headset-spezfischen Code hin und herwechseln zu können. (Scripts sollten also auf die Einstellung hier zugreifen). 

Es gibt bei genauerem Betrachten ein leichtes Flackern bei der Varjo, wenn man die Augen auf verschiedene Gegenstände fokussiert. Das liegt anscheinend an der Natur der XR-Brille, da diese einen Fokuspunkt hat (und dieser wohl entweder nicht mit Unity oder nicht perfekt mit dem anderen Display abgestimmt ist).
Dieses rendert nämlich nur einen kleinen Teilbereich scharf. Leider habe ich bis jetzt keine Einstellung/Lösung gefunden, außer generell die Option "Forveated Rendering" (die Beschränkung dieses kleinen Fokusfeldes, um Performance zu sparen) auszuschalten, mit dem Risiko, dass damit die Performance schlechter wird.

Um den einzelnen Controllern eine Funktion zuzweisen, so fügt man am besten dem jeweiligen Controller im XR-Rig ein Script hinzu, welches dann in folgender Struktur den Input abfängt (altes InputSystem - Eventbasiert):

    Controller controller;

    void Start()
    {
        controller = GetComponent<Controller>();

    }

    // Update is called once per frame
    void Update()
    {
        if (controller.Primary2DAxisClick)
        {
            Debug.Log("Controller: Primary2DAxisClick");
        }
    }


Dieses Projekt unterstützt jedoch beide Arten des Inputsystems (neues von Unity und altes), weshalb in Zukunft ein vollständiger Umstieg auf das neue möglich wäre (dies müsste getestet werden).


Hilfreiche Hinweise:
- Eine komplette Auflistung der Tasten lässt sich im Script "Controller.cs" finden.
- Um sich zu teleportieren, welches auf "primaryButton" gelegt wurde, muss bei einem HTC Controller der Button über dem Touchpad gedrückt werden. Dies funktioniert aktuell nur auf der Varjo, weil bei OpenXR die Erkennung für die HTC Vive Controller einen Bug hat. (Siehe GitHub Issue)
- Um Objekte aufzuheben und zu bewegen muss bei einem HTC Vive Controller der Trigger-Button (hinten) gedrückt werden um zum Fallenlassen, dieser wieder losgelassen werden.
- Es kann sein, dass SteamVR (Input und Tracking bei Varjo, auch wenn nicht im Projekt) etwas spinnt, wenn man im gleichen Betrieb zwischen einer Brille hin und her schalten möchte - in diesem Fall einfach den PC neu starten. Manchmal hat es in diesem Fall auch funktioniert, die Brille über Varjo Base neu zu starten und SteamVR von dort aus zu öffnen.
- Es gibt interessanterweise einen Offset zwischen dem Input Tracking von Varjo und OpenXR. Dieser Offset wurde im AR_VR_Toggle berücksichtigt. Wenn sich dieser Zustand in der zukunft ändern sollte, sollte dies dort angepasst werden