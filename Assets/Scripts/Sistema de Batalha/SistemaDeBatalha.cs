using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.VirtualTexturing;

public enum EstadoDeBatalha { Start, ActionSelection, MoveSelection, RunningTurn, Busy, PartyScreen, BattleOver }
public enum AcaoDeBatalha { Move, TrocarPokemon, UsarItem, Fugir }

public class SistemaDeBatalha : MonoBehaviour
{


    [SerializeField]  QuestController quest;
    [SerializeField]  GameController gm;
    [SerializeField]  GameObject textPokebola;

    [Header("Dialogo")]
    [Space(15)]
    [SerializeField] DialogoDeBatalha dialogBox;

    [Header("Pokemons na Batalha")]
    [Space(15)]
    Pokemon pokemonSelvagem;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    

    [Header("Pokemon na Party")]
    [Space(15)]
    [SerializeField] PartyScreen partyScreen;

    PokemonParty playerParty;
    PokemonParty trainerParty;

    [Header("Jogador")]
    [Space(15)]
    public bool PlayerCanBattle = false;
    bool PossoFugir = true;
    PlayerController player;
    GameObject pp;
    

    [Header("Treinador/Inimigo")]
    [Space(15)]
    public GameObject treinador_atual;
    bool isTrainerBattle = false;
    bool InimigoAtaca = true;

    [Header("Camera e Transicao")]
    [Space(15)]
    public GameObject batalha;
    [SerializeField] Caminhos caminhos;
    [SerializeField] LevelChanger Transitor;
    [SerializeField] LevelChanger Transitor2;

    //Outros
    public event Action<bool> OnBattleOver;
    EstadoDeBatalha state;
    EstadoDeBatalha? prevState;
    int currentAction; // acao atual
    int currentMove; // acao atual
    int currentMember;

    public int EscapeAttempts { get; set; }


    public void Start()
    {
        pp = GameObject.Find("Player");
        player = pp.GetComponent<PlayerController>();
    }

    public void StartBattle(PokemonParty playerParty, Pokemon pokemonSelvagem)
    {
        if (PlayerCanBattle)
        {
            textPokebola.GetComponentInChildren<TextMeshProUGUI>().text = "x" + gm.pokeballs.ToString("n0");
            PlayerCanBattle = false;
            //Transitor = GetComponent<LevelChanger>();
            //StartCoroutine(Transitor.Transicao());
            //Transitor.TirarVel();

            PossoFugir = true;
            InimigoAtaca = true;
            this.playerParty = playerParty;
            this.pokemonSelvagem = pokemonSelvagem;

            StartCoroutine(SetupBattle()); // Preparar para porrada
        }
        else { ResetarTudo(); }
    }

    public void ResetarTudo()
    {
        if (!PlayerCanBattle)
        {
            Debug.Log("ESTOU APAGANDO TUDO E RESETANDO VARIAVEIS");

            isTrainerBattle = false;
            PossoFugir = true;
            InimigoAtaca = true;

            this.playerParty = null;
            this.pokemonSelvagem = null;
            this.trainerParty = null;

            //Transitor.DevolverVel();

            playerUnit.DestroyInstantiatedModel(); // sumindo com os modelos 3d do player
            enemyUnit.DestroyInstantiatedModel(); // sumindo com os modelos 3d do inimigo
        }
    
    }

    public IEnumerator SetupBattle()
    {
        
        playerUnit.Setup(playerParty.GetHealthyPokemon());
        enemyUnit.Setup(pokemonSelvagem);

        partyScreen.Init();

        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);


        yield return dialogBox.TypeDialog($"Um {enemyUnit.Pokemon.Base.Nome} apareceu!");

        ActionSelection();
    }

    public void StartTrainerBattle(PokemonParty playerParty, PokemonParty trainerParty)
    {
        if (PlayerCanBattle)
        {
            PlayerCanBattle = false;
            //Transitor = GetComponent<LevelChanger>();
            //StartCoroutine(Transitor.Transicao());
            //Transitor.TirarVel();

            PossoFugir = true;
            this.playerParty = playerParty;
            this.trainerParty = trainerParty;

            isTrainerBattle = true;
            

            StartCoroutine(SetupBattleTrainer()); // Preparar para porrada
        }
        else { ResetarTudo(); }
    }
    

    public IEnumerator SetupBattleTrainer()
    {
        playerUnit.Setup(playerParty.GetHealthyPokemon());
        enemyUnit.Setup(trainerParty.GetHealthyPokemon());

        partyScreen.Init();

        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);

        yield return dialogBox.TypeDialog($"Um {enemyUnit.Pokemon.Base.Nome} apareceu!");

        ActionSelection();
    }

    void BattleOver(bool won)
    {
        //StartCoroutine(Transitor2.Transicao());

        state = EstadoDeBatalha.BattleOver;
        playerParty.Pokemons.ForEach(p => p.OnBattleOver());
        OnBattleOver(won);

        if (won == true && treinador_atual == null) { UparPokemon(); }

        playerUnit.DestroyInstantiatedModel(); // sumindo com os modelos 3d do player
        enemyUnit.DestroyInstantiatedModel(); // sumindo com os modelos 3d do inimigo
        PossoFugir = true;
        //batalha.SetActive(false);

        if (treinador_atual != null && won == true)
        {
            Debug.Log(treinador_atual);

            var _parar = treinador_atual.GetComponent<TrainerController>();

            UparPokemon();

            _parar.PerdiBatalha = true;
            _parar.posso_batalha = false;
            treinador_atual = null;
            isTrainerBattle = false;
            
        }

        



            #region TELEPORTE PÓS MORTE
            if (won == false &&  quest.centroPokemon == false)//Perdi e meus pokemons morreram = voltar pra casa
        {
            Transitor.Teleporte();
            foreach (Pokemon p in playerParty.pokemons)
            {
                p.HP = p.VidaMax;
            }
            PlayerCanBattle = true;
        }
        else if (won == false && quest.centroPokemon == true) 
                { 
                    Transitor.TeleporteCentroPokemon();
                    foreach (Pokemon p in playerParty.pokemons)
                    {
                        p.HP = p.VidaMax;
                    }
            PlayerCanBattle = true;
        }
        #endregion
    }

    public void ActionSelection()
    {
        state = EstadoDeBatalha.ActionSelection;
        dialogBox.AtivarSelecaoAcao(true);
       // StartCoroutine(dialogBox.TypeDialog("Escolha sua ação"));
        dialogBox.SetDialog("Escolha sua ação...");
    }

    
    public void UparPokemon()
    {
        //Estou batalhando com um treinador
        if (treinador_atual != null)

        {
            //RODAR XP
            foreach (Pokemon p in playerParty.pokemons)
            {
                for (var i = 0; i < trainerParty.pokemons[0].XpGiven; i++)
                {
                    p.XpAtual ++;

                    if (p.XpAtual >= p.level * 100)
                    {
                        p.XpAtual = 0;
                        p.level++;
                    }
                }

            }
        }//Estou contra um pokemon selvagem
        else if (pokemonSelvagem != null)
        {
            //RODAR XP
            foreach (Pokemon p in playerParty.pokemons)
            {
                for (var i = 0; i < pokemonSelvagem.XpGiven; i++)
                {
                    p.XpAtual ++;

                    if (p.XpAtual >= p.level * 100)
                    {
                        p.XpAtual = 0;
                        p.level++;
                    }
                }

            }
        }
        else { Debug.Log("Não estou contra pokemon selvagem nem treinador, Falhei em upar pokemon"); }
       
    }
    

    public void OpenPartyScreen()
    {
        state = EstadoDeBatalha.PartyScreen;
        partyScreen.SetPartyData(playerParty.Pokemons);
        partyScreen.gameObject.SetActive(true);
    }

    public void OpenPartyScreenAfterDead()
    {
        InimigoAtaca = false; // Inimigo nao pode me atacar apos eu trocar dps de morrer
        state = EstadoDeBatalha.PartyScreen;
        partyScreen.SetPartyData(playerParty.Pokemons);
        partyScreen.gameObject.SetActive(true);
    }

    public void MoveSelection()
    {
        
        dialogBox.AtivarSelecaoAcao(false);
        dialogBox.AtivarTextoDialogo(false);
        dialogBox.AtivarSelecaoGolpe(true);
        state = EstadoDeBatalha.MoveSelection;
    }

    public IEnumerator RunTurns(AcaoDeBatalha playerAction)
    {
        state = EstadoDeBatalha.RunningTurn;

        if (playerAction == AcaoDeBatalha.Move)
        {
           playerUnit.Pokemon.CurrentMove = playerUnit.Pokemon.Moves[currentMove];
           enemyUnit.Pokemon.CurrentMove = enemyUnit.Pokemon.GetRandomMove();

            int playerMovePriority = playerUnit.Pokemon.CurrentMove.Base.Prioridade;
            int enemyMovePriority = enemyUnit.Pokemon.CurrentMove.Base.Prioridade;

            //Checando quem vai primeiro
            bool playerGoesFirst = true;
            if (enemyMovePriority > playerMovePriority)
                playerGoesFirst = false;
            else if (enemyMovePriority == playerMovePriority)
                playerGoesFirst = playerUnit.Pokemon.Velocidade >= enemyUnit.Pokemon.Velocidade;

            var firstUnit = (playerGoesFirst) ? playerUnit : enemyUnit;
            var secondUnit = (playerGoesFirst) ? enemyUnit : playerUnit;

            var secondPokemon = secondUnit.Pokemon;

            // Primeiro Turno
            yield return RunMove(firstUnit, secondUnit, firstUnit.Pokemon.CurrentMove);
            yield return RunAfterTurn(firstUnit);
            if (state == EstadoDeBatalha.BattleOver) yield break;


            if(secondPokemon.HP > 0) 
            {
                 // Segundo Turno
                 yield return RunMove(secondUnit, firstUnit, secondUnit.Pokemon.CurrentMove);
                 yield return RunAfterTurn(secondUnit);
                 if (state == EstadoDeBatalha.BattleOver) yield break;
            }
        }
        else
        {
            if (playerAction == AcaoDeBatalha.TrocarPokemon)
            {
                var selectedPokemon = playerParty.Pokemons[currentMember];
                state = EstadoDeBatalha.Busy;
                yield return TrocarPokemon(selectedPokemon);
            }
            // Turno do inimigo

            var enemyMove = enemyUnit.Pokemon.GetRandomMove();
            yield return RunMove(enemyUnit, playerUnit, enemyMove);
            yield return RunAfterTurn(enemyUnit);

            if (state == EstadoDeBatalha.BattleOver) yield break;
        }

        if (state != EstadoDeBatalha.BattleOver)
            ActionSelection();
    }   

    IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move)
    {
        bool canRunMove = sourceUnit.Pokemon.OnBeforeMove();
        if (!canRunMove)
        {
            ShowStatusChanges(sourceUnit.Pokemon);
            yield return sourceUnit.Hud.UpdateHP();
            yield break;
        }
        yield return ShowStatusChanges(sourceUnit.Pokemon);

        move.PP--;
        yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Nome} usou {move.Base.Nome}");

        if (CheckIfMoveHits(move, sourceUnit.Pokemon, targetUnit.Pokemon))
        {
            //Reproduzir a animacao do ataque aqui 
            // -------- \\ 
            //caminhos.SetPathAndTarget(caminhos.alternatePaths[2], sourceUnit.transform); // O TARGET DA CAMERA DE CAMINHOS E CHAMADO AQUI!
            yield return new WaitForSeconds(1f);
            //caminhos.SetPathAndTarget(caminhos.alternatePaths[2], targetUnit.transform); // O TARGET DA CAMERA DE CAMINHOS E CHAMADO AQUI!
            //Reproduzir a animacao do inimigo tomando dano aqui 
            // -------- \\ 

            if (move.Base.Categoria == MoveCategory.Status)
            {
                yield return RunMoveEffects(move.Base.Efeitos, sourceUnit.Pokemon, targetUnit.Pokemon, move.Base.Alvo);
            }
            else
            {
                var damageDetails = targetUnit.Pokemon.TomarDano(move, sourceUnit.Pokemon);
                yield return targetUnit.Hud.UpdateHP();
               // yield return sourceUnit.Hud.UpdateXP();
                yield return ShowDamageDetails(damageDetails);
            }

            if (move.Base.EfeitoSecundario != null && move.Base.EfeitoSecundario.Count > 0 && targetUnit.Pokemon.HP > 0)
            {
                foreach (var secondary in move.Base.EfeitoSecundario)
                {
                    var rnd = UnityEngine.Random.Range(1, 101);
                    if(rnd <= secondary.Chance)
                        yield return RunMoveEffects(secondary, sourceUnit.Pokemon, targetUnit.Pokemon, secondary.Alvo);
                }
            }

            if (targetUnit.Pokemon.HP <= 0)
            {
                yield return dialogBox.TypeDialog($"{targetUnit.Pokemon.Base.Nome} foi derrotado");
                //Reproduzir a animacao de desmaio aqui 
                // ---------\\ 
                yield return new WaitForSeconds(2f);

                targetUnit.DestroyInstantiatedModel();
                //sourceUnit.Hud.UpdateXP();

                CheckForBattleOver(targetUnit);
            }
        }
        else
        {
            yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Nome} errou o ataque");
        }
    }

    IEnumerator RunMoveEffects(MoveEffects efeito, Pokemon source, Pokemon target, MoveTarget moveTarget)
    {
        
        // Stat Boosting
        if (efeito.Boosts != null)
        {
            if (moveTarget == MoveTarget.Proprio)
                source.ApplyBoosts(efeito.Boosts);
            else
                target.ApplyBoosts(efeito.Boosts);
        }

        // Condição de Status 
        if (efeito.Status != ConditionID.none)
        {
            target.SetStatus(efeito.Status);
        }

        // Condição de Status Volatil
        if (efeito.VolatileStatus != ConditionID.none)
        {
            target.SetVolatileStatus(efeito.VolatileStatus);
        }

        yield return ShowStatusChanges(source);
        yield return ShowStatusChanges(target);
    }

    IEnumerator RunAfterTurn(BattleUnit sourceUnit)
    {
        if (state == EstadoDeBatalha.BattleOver) yield break;
        yield return new WaitUntil(() => state == EstadoDeBatalha.RunningTurn);

        // Status como queimar ou envenenar pokemon depois do turno
        sourceUnit.Pokemon.OnAfterTurn();
        yield return ShowStatusChanges(sourceUnit.Pokemon);
        yield return sourceUnit.Hud.UpdateHP();

        if (sourceUnit.Pokemon.HP <= 0)
        {
            yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Nome} foi derrotado");
            //Reproduzir a animacao de desmaio aqui 
            // ---------\\ 
            yield return new WaitForSeconds(2f);

            sourceUnit.DestroyInstantiatedModel();

            CheckForBattleOver(sourceUnit);
        }
    }

    bool CheckIfMoveHits(Move move, Pokemon source, Pokemon target)
    {
        if (move.Base.SempreAcerta)
            return true;

        float moveAccuracy = move.Base.Precisao;

        int accuracy = source.StatBoosts[Stat.Accuracy];
        int evasion = target.StatBoosts[Stat.Evasion];

        var boostValues = new float[] { 1f, 4f / 3f, 5f / 3f, 2f, 7f / 3f, 8f / 3f, 3f };

        //Precisão

        if (accuracy > 0)
            moveAccuracy *= boostValues[accuracy];
        else
            moveAccuracy /= boostValues[-accuracy];

        // Evasiva

        if (evasion > 0)
            moveAccuracy /= boostValues[evasion];
        else
            moveAccuracy *= boostValues[-evasion];

        return UnityEngine.Random.Range(1, 101) <= moveAccuracy;
    }
    IEnumerator ShowStatusChanges(Pokemon pokemon)
    {
        while (pokemon.StatusChanges.Count > 0)
        {
            var message = pokemon.StatusChanges.Dequeue();
            yield return dialogBox.TypeDialog(message);
        }
    }

    void CheckForBattleOver(BattleUnit faintedUnit)
    {
        if (faintedUnit.IsPlayerUnit)
        {
            var nextPokemon = playerParty.GetHealthyPokemon();
            if (nextPokemon != null)
            {
                OpenPartyScreenAfterDead();
            }
            else
            {
                PlayerCanBattle = false;
                BattleOver(false);
            }
        }
        else
        {
            BattleOver(true);

            PlayerCanBattle = true;
        }
    }
    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1f)
        {
            yield return dialogBox.TypeDialog("Um acerto crítico!");
        }
          
        if (damageDetails.TypeEffectiveness > 1f)
        {
            yield return dialogBox.TypeDialog("É super efetivo!");
        }
        else if (damageDetails.TypeEffectiveness < 1f)
        {
            yield return dialogBox.TypeDialog("Não é muito efetivo!");
        }
          
            
    }

    public void HandleUpdate()
    {
        //Debug.Log(state);

        if (state == EstadoDeBatalha.ActionSelection)
        {
            HandleActionSelection();
            //ActionSelection();
        }
        else if (state == EstadoDeBatalha.MoveSelection)
        {
            HandleMoveSelection();
        }
        else if (state == EstadoDeBatalha.PartyScreen)
        {
            HandlePartySelection();
        }
    }

    void HandleActionSelection()
    {
        #region setous
        /*
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentAction;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentAction;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentAction += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentAction -= 2;
        */

        /*
        if (Input.GetKeyDown(KeyCode.Space) || (Input.GetKeyDown(KeyCode.Return)))
        {
            if (currentAction == 0)
            {
                // Lutar
                MoveSelection();
            }
            else if (currentAction == 1)
            {
                // Mochila
                Debug.Log("clicou para abrir a mochila");
            }
            else if (currentAction == 2)
            {
                // Pokemon
                prevState = state;
                OpenPartyScreen();
            }
            else if (currentAction == 3)
            {
                // Fugir
                Fuga();
                Debug.Log("clicou para fugir!");
            }

        

        }

        
        currentAction = Mathf.Clamp(currentAction, 0, 3);

        dialogBox.UpdateActionSelection(currentAction);

        */
        #endregion

        dialogBox.UpdateActionSelection(currentAction);
    }


    void HandleMoveSelection()
    {
        #region seta
        //  if (Input.GetKeyDown(KeyCode.RightArrow))
        //      ++currentMove;
        //  else if (Input.GetKeyDown(KeyCode.LeftArrow))
        //      --currentMove;
        //  else if (Input.GetKeyDown(KeyCode.DownArrow))
        //      currentMove += 2;
        //  else if (Input.GetKeyDown(KeyCode.UpArrow))
        //      currentMove -= 2;
        //
        //  currentMove = Mathf.Clamp(currentMove, 0, playerUnit.Pokemon.Moves.Count - 1);
        //
        //    dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);

        /*
        if (Input.GetKeyDown(KeyCode.Space) || (Input.GetKeyDown(KeyCode.Return)))
        {
            #region seta2
            // var move = playerUnit.Pokemon.Moves[currentMove];
            // if (move.PP == 0) return;
            // dialogBox.AtivarSelecaoGolpe(false);
            // dialogBox.AtivarTextoDialogo(true);

            // StartCoroutine(RunTurns(AcaoDeBatalha.Move));
            #endregion
            GolpeUI();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            /*
            dialogBox.AtivarSelecaoGolpe(false);
            dialogBox.AtivarTextoDialogo(true);
            ActionSelection();
            */
        //}
        

        #endregion

    }

    public void GolpeUI()
    {
        var move = playerUnit.Pokemon.Moves[currentMove];
        if (move.PP == 0) return;
        dialogBox.AtivarSelecaoGolpe(false);
        dialogBox.AtivarTextoDialogo(true);
        StartCoroutine(RunTurns(AcaoDeBatalha.Move));
    }

    #region Gambiarra

    public void S1()
    {
        //new WaitForSecond(2f);
        currentMove = 0;
        currentMember = 0;
    }
    public void S2()
    {
        currentMove = 1;
        currentMember = 1;
    }

    public void S3()
    {
        currentMove = 2;
        currentMember = 2;
    }

    public void S4()
    {
        currentMove = 3;
        currentMember = 3;
    }

    public void S5()
    {
     //   currentMove = 3;
        currentMember = 4;
    }

    public void S6()
    {
     //   currentMove = 3;
        currentMember = 5;
    }

    public void SairParty()
    {
        partyScreen.gameObject.SetActive(false);
        ActionSelection();
    }

    public void SairGolpe()
    {
        dialogBox.AtivarSelecaoGolpe(false);
        ActionSelection();
    }

    #endregion

    void HandlePartySelection()
    {
        #region SETAAAS
        /*
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentMember;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentMember;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentMember += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentMember -= 2;
        */

        /*
        if (Input.GetKeyDown(KeyCode.Space) || (Input.GetKeyDown(KeyCode.Return)))
        {
         //   var selectedMember = playerParty.Pokemons[currentMember];
            if (playerParty.Pokemons[currentMember].HP <= 0)
            {
                partyScreen.SetMessageText("Você não pode enviar um pokemon desmaiado.");
                return;
            }

            if (playerParty.Pokemons[currentMember] == playerUnit.Pokemon)
            {
                partyScreen.SetMessageText("Você não pode trocar para o mesmo pokemon.");
                return;
            }

            partyScreen.gameObject.SetActive(false);

            if (prevState == EstadoDeBatalha.ActionSelection)
            {
                prevState = null;
                StartCoroutine(RunTurns(AcaoDeBatalha.TrocarPokemon));
            }
            else
            {
                state = EstadoDeBatalha.Busy;
                Troca();
            }

        }

        
        currentMember = Mathf.Clamp(currentMember, 0, playerParty.Pokemons.Count - 1);

        partyScreen.UpdateMemberSelection(currentMember);

        */
        #endregion

    }

    public void Troca()
    {
        if (playerParty.Pokemons[currentMember].HP <= 0)
        {
            partyScreen.SetMessageText("Você não pode enviar um pokemon desmaiado.");
            return;
        }
        else
        if (playerParty.Pokemons[currentMember] == playerUnit.Pokemon)
        {
            partyScreen.SetMessageText("Você não pode trocar para o mesmo pokemon.");
            return;
        }
        else 
        {
            //Desativando canvas
            dialogBox.AtivarSelecaoAcao(false);

            StartCoroutine(TrocarPokemon(playerParty.Pokemons[currentMember]));
        }

    }
    public void Fuga()
    {
        //Debug.Log(TryToEscape());

        if (PossoFugir)
        {
            dialogBox.AtivarSelecaoAcao(false);
            StartCoroutine(TryToEscape());
        }
    }

    public IEnumerator TryToEscape()
    {
        PossoFugir = false;

        if (isTrainerBattle)
        {
         yield return dialogBox.TypeDialog($"Você não pode fugir de batalhas com treinadores!");
         yield return new WaitForSeconds(2f);
         PossoFugir = true;

        // Turno do inimigo

        state = EstadoDeBatalha.RunningTurn;

        partyScreen.gameObject.SetActive(false);

        //Desativando canvas
        dialogBox.AtivarSelecaoAcao(false);

        var enemyMove = enemyUnit.Pokemon.GetRandomMove();
        yield return RunMove(enemyUnit, playerUnit, enemyMove);
        yield return RunAfterTurn(enemyUnit);
        yield return new WaitForSeconds(2f);
        
        ActionSelection();
        yield break;
        }
     
      ++EscapeAttempts;

        int playerSpeed = playerUnit.Pokemon.Velocidade;
        int enemySpeed = enemyUnit.Pokemon.Velocidade;

        if (enemySpeed < playerSpeed)
        {
            dialogBox.SetDialog($"Fugiu sem problemas!");
          
            /* bs.*/
            
            playerUnit.DestroyInstantiatedModel(); // sumindo com os modelos 3d do player
            enemyUnit.DestroyInstantiatedModel(); // sumindo com os modelos 3d do inimigo

            BattleOver(true);
        }
        else
        {
            float f = (playerSpeed * 128) / enemySpeed + 30 * EscapeAttempts;
            f = f % 256;

            if (UnityEngine.Random.Range(0, 256) < f)
            {
                dialogBox.AtivarSelecaoAcao(false);
                dialogBox.SetDialog($"Escapou em segurança!");
                yield return new WaitForSeconds(2f);
                
                playerUnit.DestroyInstantiatedModel(); // sumindo com os modelos 3d do player
                enemyUnit.DestroyInstantiatedModel(); // sumindo com os modelos 3d do inimigo

                BattleOver(true);
            }
            else
            {
                dialogBox.SetDialog($"Não conseguiu escapar...");
                yield return new WaitForSeconds(2f);
                PossoFugir = true;

                // Turno do inimigo

                state = EstadoDeBatalha.RunningTurn;
                //partyScreen.gameObject.SetActive(false);

                //Desativando canvas
                dialogBox.AtivarSelecaoAcao(false);

                var enemyMove = enemyUnit.Pokemon.GetRandomMove();
                yield return RunMove(enemyUnit, playerUnit, enemyMove);
                yield return RunAfterTurn(enemyUnit);
                yield return new WaitForSeconds(2f);

                ActionSelection();
                yield break;
            }
        }
    }

    IEnumerator TrocarPokemon(Pokemon newPokemon)
    {

        partyScreen.gameObject.SetActive(false);

        if (playerUnit.Pokemon.HP > 0)
        {
            dialogBox.SetDialog($"Volte {playerUnit.Pokemon.Base.Nome}");
            // animação do pokemon indo de base vai aqui
            playerUnit.DestroyInstantiatedModel();

            //Desativando canvas
            dialogBox.AtivarSelecaoAcao(false);
            yield return new WaitForSeconds(2f); 
        }

        playerUnit.Setup(newPokemon);
        dialogBox.SetMoveNames(newPokemon.Moves);
        //Desativando canvas
        dialogBox.AtivarSelecaoAcao(false);
        dialogBox.SetDialog($"Vai {newPokemon.Base.Nome}!");

        yield return new WaitForSeconds(2f);

        partyScreen.gameObject.SetActive(false);

        // Turno do inimigo se ele conseguir
        if (InimigoAtaca)
        {
            state = EstadoDeBatalha.RunningTurn;

            //Desativando canvas
            dialogBox.AtivarSelecaoAcao(false);


            var enemyMove = enemyUnit.Pokemon.GetRandomMove();
            yield return RunMove(enemyUnit, playerUnit, enemyMove);
            yield return RunAfterTurn(enemyUnit);

            yield return new WaitForSeconds(2f);

            ActionSelection();
        }
        else //Inimigo nao esta podendo atacar
        {
            state = EstadoDeBatalha.RunningTurn;

            //Desativando canvas
            dialogBox.AtivarSelecaoAcao(false);

            yield return new WaitForSeconds(2f);

            ActionSelection();

            InimigoAtaca = true;
        }

        

        

        //yield break;
    }
}
