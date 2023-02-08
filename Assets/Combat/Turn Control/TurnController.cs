using System.Collections;
using System.Collections.Generic;
using Assets.Combat;
using Assets.EventSystem;
using TMPro;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    [Header("Serialized Object References")]
    [SerializeField] private ProgressTracker progressTracker;
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI turnLabel;
    [SerializeField] private EndCombatPanel endCombatPanel;
    [Header("Scene References")]
    [SerializeField] private PlayerInstance playerInstance;
    [SerializeField] private EnemyInstance enemyInstance;
    [SerializeField] private PathController pathController;
    [Header("Turn Flow")]
    [SerializeField] private float timeBetweenActions;
    public bool isPlayerTurn;
    public bool combatIsEnded;
    private bool awaitingAnimation;
    private float timeSinceLastAction;
    public TurnStage turnStage;
    [Header("Flow Event References")]
    [SerializeField] private StartCombatAnimationEvent startCombatAnimationEvent;
    [SerializeField] private EndCombatAnimationEvent endCombatAnimationEvent;

    private void Start()
    {
        isPlayerTurn = true;
        UpdateTurnLabel();
    }
    private void OnEnable()
    {
        startCombatAnimationEvent.AddListener(OnStartCombatAnimation);
        endCombatAnimationEvent.AddListener(OnEndCombatAnimation);
    }
    private void OnDisable()
    {
        startCombatAnimationEvent.RemoveListener(OnStartCombatAnimation);
        endCombatAnimationEvent.RemoveListener(OnEndCombatAnimation);
    }
    private void Update()
    {
        if (awaitingAnimation)
            return;
        if (timeSinceLastAction < timeBetweenActions)
        {
            timeSinceLastAction += Time.deltaTime;
            return;
        }
        if (combatIsEnded)
            return;
        if (!isPlayerTurn)
        {
            switch (turnStage)
            {
                case (TurnStage.CharacterActing):
                    if (enemyInstance.PerformNextSpell())
                        timeSinceLastAction = 0;
                    else
                    {
                        enemyInstance.RestoreMana();
                        turnStage = TurnStage.ProjectilesMoving;
                        pathController.ResetPathIndex();
                    }
                    break;
                case (TurnStage.ProjectilesMoving):
                    if (pathController.AdvanceNextPlayerProjectile())
                        timeSinceLastAction = 0;
                    else
                    {
                        turnStage = TurnStage.ShieldsExpiring;
                        pathController.ResetPathIndex();
                    }
                    break;
                case (TurnStage.ShieldsExpiring):
                    if (pathController.AdvanceNextEnemyShield())
                        timeSinceLastAction = 0;
                    else
                    {
                        isPlayerTurn = true;
                        UpdateTurnLabel();
                        turnStage = TurnStage.CharacterActing;
                    }
                    break;
            }
        }
        else
        {
            switch (turnStage)
            {
                case (TurnStage.ProjectilesMoving):
                    if (pathController.AdvanceNextEnemyProjectile())
                        timeSinceLastAction = 0;
                    else
                    {
                        turnStage = TurnStage.ShieldsExpiring;
                        pathController.ResetPathIndex();
                    }
                    break;
                case (TurnStage.ShieldsExpiring):
                    if (pathController.AdvanceNextPlayerShield())
                        timeSinceLastAction = 0;
                    else
                    {
                        isPlayerTurn = false;
                        turnStage = TurnStage.CharacterActing;
                        UpdateTurnLabel();
                        timeSinceLastAction = 0;
                        enemyInstance.StartTurn();
                    }
                    break;
            }
        }
    }
    public void UpdateTurnLabel()
    {
        string activeCharacter = isPlayerTurn ? playerInstance.characterName : enemyInstance.characterName;
        turnLabel.text = activeCharacter + "'s Turn";
    }

    public void EndPlayerTurn()
    {
        if (turnStage == TurnStage.CharacterActing & isPlayerTurn)
        {
            turnStage = TurnStage.ProjectilesMoving;
            pathController.ResetPathIndex();
            timeSinceLastAction = 0;
            playerInstance.RestoreMana();
        }

    }
    
    public void EndCombat(bool isPlayerWinner)
    {
        combatIsEnded = true;
        endCombatPanel.gameObject.SetActive(true);
        if (isPlayerWinner)
        {
            progressTracker.AddDefeatedEnemy(enemyInstance.enemyID);
            endCombatPanel.ShowVictory();
        }
        else
        {
            endCombatPanel.ShowDefeat();
        }
    }
    private void OnStartCombatAnimation(object sender, EventParameters args)
    {
        awaitingAnimation = true;
    }
    private void OnEndCombatAnimation(object sender, EventParameters args)
    {
        awaitingAnimation = false;
    }
    public enum TurnStage
    {
        CharacterActing,
        ProjectilesMoving,
        ShieldsExpiring
    }
}