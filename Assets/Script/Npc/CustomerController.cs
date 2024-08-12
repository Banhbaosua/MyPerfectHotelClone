using MonsterLove.StateMachine;
using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.AI;



public class CustomerController : MonoBehaviour
{
    enum CustomerState
    {
        AnyState,
        Wait,
        Moving,
        Sleep,
        TakingToilet,
    }
    public class CustomerDriver
    {
        public StateEvent Update;
        public StateEvent FixedUpdate;
    }
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float cash;
    [SerializeField] float tip;
    [SerializeField] RoomsData roomsData;
    [SerializeField] Rigidbody rb;
    private StateMachine<CustomerState, CustomerDriver> sfm;
    private Customer customer;
    private CustomerState currentState;
    public bool IsWaiting => currentState == CustomerState.Wait;
    Subject<Unit> onGoOut;
    public IObservable<Unit> OnGoOut => onGoOut;
    CompositeDisposable disposables = new CompositeDisposable();
    private void Awake()
    {
        sfm = new StateMachine<CustomerState, CustomerDriver>(this);
        onGoOut = new Subject<Unit>();
    }
    private void OnEnable()
    {
        customer = new Customer(cash,tip);

        CustomerManager.Instance.OnCustomerDesSet
            .First()
            .Subscribe(x =>
            {
                agent.SetDestination(x.position); 
            }).AddTo(disposables);

        sfm.ChangeState(CustomerState.Moving);
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

    void AnyState_Enter()
    {
        currentState = CustomerState.AnyState;
    }
    void AnyState_Update()
    {
        if(!customer.HasRoom)
        {
            sfm.ChangeState(CustomerState.Wait);
        }
        if (customer.HasRoom && !customer.HasSlept)
        {
            var room = customer.AsignedRoom as SleepingRoom;
            agent.SetDestination(room.Bed.position);
            sfm.ChangeState(CustomerState.Moving);
        }
        if (customer.NeedToilet)
        {
            var room = roomsData.FindAvailableByType<ToiletRoom>();
            if (room != null)
            {
                var toilet = room.GetEmptyToilet();
                customer.SetToilet(toilet);
                toilet.Occupied();
                AsignRoom(room);
                agent.SetDestination(toilet.transform.position);
            }
            else
            {
                agent.SetDestination(CustomerManager.Instance.StartPos.position);
            }
            sfm.ChangeState(CustomerState.Moving);
        }

        if(customer.Done)
        {
            agent.SetDestination(CustomerManager.Instance.StartPos.position);
            sfm.ChangeState(CustomerState.Moving);
        }
    }
    void Moving_Enter()
    {
        currentState = CustomerState.Moving;
        agent.isStopped = false;
    }

    
    void Moving_Update()
    {
        if (agent.remainingDistance < 0.1f)
        {
            sfm.ChangeState(CustomerState.AnyState);
            if (customer.HasRoom && !customer.HasSlept)
                sfm.ChangeState(CustomerState.Sleep);
            if (customer.NeedToilet)
                sfm.ChangeState(CustomerState.TakingToilet);
            if (customer.Done)
            {
                onGoOut.OnNext(Unit.Default);
            }
        }
    }

    void Sleep_Enter()
    {
        currentState = CustomerState.Sleep;
        rb.isKinematic = true;
        agent.isStopped = true;
        agent.enabled = false;

        this.transform.position = customer.GetRoom<SleepingRoom>().SleepPos.position;
        this.transform.rotation = Quaternion.Euler(-90, 180, 0);
        StartCoroutine(BehaviourTimer(3));
    }

    void Sleep_Exit()
    {
        rb.isKinematic = false;
        this.transform.rotation = Quaternion.identity;
        this.transform.position = customer.GetRoom<SleepingRoom>().Bed.position;
        agent.enabled = true;

        customer.Slept();

        //Random give tip
        //if (RandomByWeight(50))
        //{
        //    GiveMoney(tip);
        //}
        //random need toilet
        if(RandomByWeight(100))
        {
            customer.RequestToilet(true);
            
        }
    }

    void TakingToilet_Enter()
    {
        StopAgent(true);
        this.transform.rotation = Quaternion.Euler(0, 180, 0);
        this.transform.position = customer.CurrentToilet.SitSpot.position;
        StartCoroutine(BehaviourTimer(3));

    }

    void TakingToilet_Exit()
    {
        rb.isKinematic = false;
        customer.Out();
        customer.RequestToilet(false);
        this.transform.rotation = Quaternion.identity;
        this.transform.position = customer.GetRoom<ToiletRoom>().transform.position;
        agent.enabled = true;
    }

    void Wait_Enter()
    {
        currentState = CustomerState.Wait;
        agent.isStopped = true;
    }

    void Wait_Update()
    {
        if (customer.HasRoom)
        {
            sfm.ChangeState(CustomerState.AnyState);
        }
        if (agent.remainingDistance > 0.1f)
            sfm.ChangeState(CustomerState.Moving);
    }

    bool RandomByWeight(float weight) 
    {
        var rd = UnityEngine.Random.Range(0, 100f);
        if(rd- weight <= 0)
            return true;
        else
            return false;
    }

    void GiveMoney(float cash)
    {

    }

    public void AsignRoom(Room room)
    {
        customer.SetRoom(room);

    }

    public void SetDest(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    IEnumerator BehaviourTimer(float sleepTime)
    {
        float timer = sleepTime;
        while(timer > 0)
        {
            yield return new WaitForEndOfFrame();
            timer-= Time.deltaTime;
        }

        sfm.ChangeState(CustomerState.AnyState);
    }

    void StopAgent(bool value)
    {
        rb.isKinematic = value;
        agent.isStopped = value;
        agent.enabled = !value;
    }
}
