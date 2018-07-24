using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

	public static ObjectPooler m_SharedInstance;
	public List<GameObject> m_pooledObjects;
	public GameObject m_objectToPool;
	public int m_amountToPool;

	void Awake()
	{
		m_SharedInstance = this;
	}

	void Start()
	{
		m_pooledObjects = new List<GameObject>();
		for(int i = 0; i < m_amountToPool; i++)
		{
			GameObject obj = (GameObject)Instantiate(m_objectToPool);
			obj.SetActive(false); 
			m_pooledObjects.Add(obj);
		}
	}

	public GameObject GetPooledObject() {
		for (int i = 0; i < m_pooledObjects.Count; i++) {
			if (!m_pooledObjects[i].activeInHierarchy) {
				return m_pooledObjects[i];
			}
		}
		return null;
	}
}
