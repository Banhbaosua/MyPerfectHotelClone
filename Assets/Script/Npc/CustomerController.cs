using MonsterLove.StateMachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

public enum CustomerState
{
    AnyState,
    Waiting,
    Moving,
    Sleeping,
}

public class CustomerDriver
{
    public StateEvent Update;
    public StateEvent FixedUpdate;
}
public class CustomerController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float cash;
    [SerializeField] float tip;
    private StateMachine<CustomerState, CustomerDriver> sfm;
    private Customer customer;

    CompositeDisposable disposables = new CompositeDisposable();
    private void OnEnable()
    {
        sfm = new StateMachine<CustomerState, CustomerDriver>(this);
        customer = new Customer(cash,tip);

        CustomerManager.Instance.OnCustomerDesSet
            .First()
            .Subscribe(x =>
            {
                agent.SetDestination(x.position); 
            }).AddTo(disposables);

        sfm.ChangeState(CustomerState.Moving);
        agent.stoppingDistance = 0;

    }

    private void Update()
    {
        sfm.Driver.Update.Invoke();
    }

    private void FixedUpdate()
    {
        sfm.Driver.FixedUpdate.Invoke();
    }

    private void OnDisable()
    {
        disposables?.Clear();
    }

    void AnyState_Update()
    {
        if (!customer.HasRoom && agent.remainingDistance < 0.01f)
        {
            sfm.ChangeState(CustomerState.Waiting);
        }
        if (!customer.HasSlept && agent.remainingDistance < 0.01f)
        {
            sfm.ChangeState(CustomerState.Sleeping);
        }
        if (customer.HasRoom && !customer.HasSlept)
        {
            var room = customer.AsignedRoom as SleepingRoom;
            agent.SetDestination(room.Bed.position);
            sfm.ChangeState(CustomerState.Moving);
        }
        if(customer.HasSlept)
        {
            var giveTip = RandomGiveMoney(50f);
            if(giveTip)
            {
                GiveMoney(customer.Tip);
            }
        }

    }
    void Moving_Enter()
    {
        agent.isStopped = false;
    }

    void Moving_FixedUpdate()
    {
        if (agent.remainingDistance <0.01f)
            sfm.ChangeState(CustomerState.AnyState);
    }

    void Sleep_Enter()
    {
        agent.isStopped = true;
        StartCoroutine(Sleep());
    }

    void Waiting_Enter()
    {
        agent.isStopped = true;
        Debug.Log("Waiting enter");
    }

    void Waiting_Update()
    {
        if(customer.HasRoom)
        {
            var room = customer.AsignedRoom as SleepingRoom;
            agent.SetDestination(room.Bed.position);
            sfm.ChangeState(CustomerState.Moving);
        }
    }

    bool RandomToiletUse(float weight)
    {
        var rd = Random.Range(0, 100);
        if (rd - weight < 0)
            return true;
        else
            return false;
    }

    bool RandomGiveMoney(float weight) 
    {
        var rd = Random.Range(0, 100);
        if(rd- weight < 0)
            return true;
        else
            return false;
    }

    void GiveMoney(float cash)
    {

    }

    IEnumerator Sleep()
    {
        this.transform.rotation = Quaternion.Euler(-90, 180, 0);
        yield return new WaitForSeconds(5f);

        this.transform.rotation = Quaternion.identity;
        customer.Slept();
        sfm.ChangeState(CustomerState.AnyState);
    }
}
