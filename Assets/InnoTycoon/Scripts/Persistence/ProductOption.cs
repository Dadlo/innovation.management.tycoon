using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// uma opcao de criacao de um produto.
/// opcoes possuem um titulo, id, custo, possiveis requisitos e um multiplicador
/// </summary>
[System.Serializable]
public class ProductOption {
	public string title;
	public string id;
	public int requisitions;
    public int cost;
	public float multiplier;
	public bool active;

	public ProductOption(){}
}
