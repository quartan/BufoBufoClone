using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ProductGUI : MonoBehaviour
{
    [SerializeField] private Button buyButton;
    [SerializeField] private Text headerText;
    [SerializeField] private Text countPriceChangeText;
    [SerializeField] private Text countProductText;
    [SerializeField] private Image avatarChange;
    [SerializeField] private Image avatarPriceChange;
    [SerializeField] private Image background;
    private Product product;
    private Action ActionRemove;

    public void Init(Action<Product> ActionBuy, Action ActionRemove, Product product)
    {
        UpdateData(product);

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() =>
        {
            ActionBuy?.Invoke(product);
        });
        this.ActionRemove = ActionRemove;
    }

    public void UpdateData(Product product)
    {
        this.product = product;
        headerText.text = product.header;
        avatarChange.sprite = product.avatarChange;
        avatarPriceChange.sprite = product.avatarPriceChange;
        if (product.countChangeProduct == -1)
            countProductText.gameObject.SetActive(false);
        else
        {
            if (product.countChangeProduct == 0)
            {
                print(2);
                ActionRemove?.Invoke();
                return;
            }
        }

        countProductText.text = $"{product.countChangeProduct}x";
        countPriceChangeText.gameObject.SetActive(true);
        countPriceChangeText.text = $"{product.countPriceChange}x";
    }

    public Product GetProduct()
    {
        return product;
    }
}
