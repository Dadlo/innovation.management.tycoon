using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public class AITycoon
{
    public string name;

    public List<Product> products;

    public Product productDoing;

    public List<string> studiesDone;

    public string studyDoing;

    public int curStudyStep = 0;

    public int curIncome = 0;

    public int curMoney = 0;

    public int weeksDownRemaining = 0;

    /// <summary>
    /// quanto maior esse valor (0 a 1), maior a chance desse cara fazer estudos e produtos diferentes ao inves de fazer um igual toda vez
    /// </summary>
    public float intelligence = 0;

    public AITycoon()
    {
        if(studiesDone == null)
        {
            studiesDone = new List<string>();
        }

        if(products == null)
        {
            products = new List<Product>();
        }
    }

    public void Tick()
    {
        if(weeksDownRemaining > 0)
        {
            weeksDownRemaining--;
            if(weeksDownRemaining <= 0)
            {
                ModalPanel.Instance().OkBox(GameManager.GetVariableText("aiBackToBusinessHdr"), GameManager.GetVariableText("aiBackToBusinessTxt", name));
                curMoney = GameManager.aiMoneyWhenRespawned + Random.Range(0, GameManager.aiMoneyWhenRespawned / 2);
            }
            else
            {
                return;
            }
        }

        curMoney += curIncome;

        if(curMoney <= 0)
        {
            GoBankrupt();
            return;
        }

        if (!string.IsNullOrEmpty(studyDoing))
        {
            curStudyStep++;
            if(curStudyStep >= GameManager.instance.GetStudyByName(studyDoing).steps)
            {
                //study done!
                curIncome += GameManager.instance.GetStudyByName(studyDoing).cost;
                studiesDone.Add(studyDoing);
                studyDoing = "";
                curStudyStep = 0;
            }
        }
        else
        {
            if(Random.Range(0f, 1.0f) < intelligence)
            {
                DoNewStudy();
            }
        }

        if (productDoing != null && productDoing.owner == name)
        {
            productDoing.OneStep();
        }
        else
        {
            DoNewProduct();
        }


    }

    public void DoNewStudy()
    {
        if (string.IsNullOrEmpty(studyDoing))
        {
            IEnumerable<string> availableStudies = GameManager.instance.studyNames.Except(studiesDone);
            //se ja estudamos tudo, deixa pra la haha
            if (availableStudies.Count() > 0)
            {
                //pegamos um estudo que ainda nao estudamos
                StudyOption chosenStudy = GameManager.instance.GetStudyByName(availableStudies.ElementAt(Random.Range(0, availableStudies.Count())));
                //se tivermos dinheiro, estudamos
                if(curMoney > GameManager.aiSafeAmountWhenStudying + ((GameManager.baseDailyCost + chosenStudy.cost) * chosenStudy.steps))
                {
                    studyDoing = chosenStudy.title;
                    curIncome -= chosenStudy.cost;
                }
                
            }
        }
    }

    public void DoNewProduct()
    {
        Product createdProduct = new Product();
        createdProduct.name = name; //TODO algum esqueminha pra gerar nomes?
        createdProduct.owner = name;

        //se tivermos nome repetido, lancar o 2, ou 3...
        if (GameManager.instance.GetProductByName(createdProduct.name) != null)
        {
            int sequelNumber = 2;
            while (GameManager.instance.GetProductByName(string.Concat(createdProduct.name, " ", sequelNumber.ToString())) != null)
            {
                sequelNumber++;
            }
            createdProduct.name = string.Concat(createdProduct.name, " ", sequelNumber.ToString());
        }

        createdProduct.pickedOptionIDs = new List<string>();

        createdProduct.pickedOptionIDs.Add("ino_01"); //mesmo se nenhuma opcao for escolhida, essa ja vem haha

        createdProduct.conceptSteps = GameManager.baseNumberOfConceptSteps;
        createdProduct.devSteps = GameManager.baseNumberOfDevSteps;
        createdProduct.saleSteps = GameManager.baseNumberOfSaleSteps;

        for (int i = 0; i < studiesDone.Count; i++)
        {
            if(Random.Range(0, 1.0f) < intelligence)
            {
                string skillId = GameManager.instance.GetStudyByName(studiesDone[i]).skillId;
                
                //adicionamos os passos (tudo em concepcao caso nao seja marketing)
                ProductOption chosenOption = GameManager.instance.GetProductOptionByID(skillId);

                createdProduct.pickedOptionIDs.Add(skillId);

                int projectedPaidTime = GameManager.CalculateProductPaidTime(createdProduct);

                //se pudermos pagar, mantemos essa opcao
                if (curMoney - GetMoneyThatWillBeSpentUntilStudyEnd() -
                    ((GameManager.baseDailyCost * projectedPaidTime) + GameManager.CalculateProductTotalCost(createdProduct)) >
                    GameManager.aiSafeAmountWhenDesigningProduct)
                {
                    switch (GameManager.instance.GetProductOptionPhase(chosenOption))
                    {

                        case Product.ProductPhase.concept:
                            createdProduct.conceptSteps += Mathf.Max(0, chosenOption.multiplier - 1);
                            break;
                        case Product.ProductPhase.dev:
                            createdProduct.devSteps += Mathf.Max(0, chosenOption.multiplier - 1);
                            break;
                        case Product.ProductPhase.sales:
                            createdProduct.saleSteps += Mathf.Max(0, chosenOption.multiplier - 1);
                            break;
                    }
                }
                else
                {
                    createdProduct.pickedOptionIDs.Remove(skillId);
                }

            }
        }

        createdProduct.conceptFocusPercentage = 0.33f;
        createdProduct.devFocusPercentage = 0.33f;
        createdProduct.saleFocusPercentage = 0.34f;

        curIncome -= GameManager.CalculateProductDailyCost(createdProduct);
        productDoing = createdProduct;
        products.Add(createdProduct);
    }

    public void GoBankrupt()
    {
        ModalPanel.Instance().OkBox(GameManager.GetVariableText("aiBankruptedHdr"), GameManager.GetVariableText("aiBankruptedTxt", name));
        weeksDownRemaining = GameManager.aiWeeksOutWhenBankrupted + Random.Range(0, GameManager.aiWeeksOutWhenBankrupted);
    }

    int GetMoneyThatWillBeSpentUntilStudyEnd()
    {
        if (!string.IsNullOrEmpty(studyDoing))
        {
            StudyOption curStudy = GameManager.instance.GetStudyByName(studyDoing);
            return (curStudy.steps - curStudyStep) * curStudy.cost;
        }
        else
        {
            return 0;
        }
        
    }
}
