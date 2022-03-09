Was zu beachten ist:
Sky and Fog Volume: Ich habe die Sky and Fog Volume leicht angepasst, sodass sie die Voraussetzungen für die Varjo SDK erfüllen  (Fokus-Rechteck sollte dadurch verschwunden sein). Wenn die Helligkeit nicht passt - eifnach unter Exposure anpassen.

DeviceManager: Dieses Objekt ist dazu gedacht, zwischen Headset-spezfischen Code hin und herwechseln zu können. (Scripts sollten also auf die Einstellung hier zugreifen). 

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


Eine komplette Auflistung der Tasten lässt sich im Script "Controller.cs" finden.

Hilfreiche Hinweise:
- Um sich zu teleportieren, welches auf "primaryButton" gelegt wurde, muss bei einem HTC Controller der Button über dem Touchpad gedrückt werden.
- Um Objekte aufzuheben und zu bewegen muss bei einem HTC Vive Controller der Trigger-Button (hinten) gedrückt werden um zum Fallen lassen