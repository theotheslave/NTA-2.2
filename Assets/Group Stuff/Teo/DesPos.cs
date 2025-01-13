using UnityEngine;

public class DesPos : MonoBehaviour
{

    private bool isCentred = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool CanCentre()
    {

        return !isCentred;

    }

    public void Centred()
    {
        isCentred = true;

    }

    public void notCentred()
    {
        isCentred = false;

    }
    
}
