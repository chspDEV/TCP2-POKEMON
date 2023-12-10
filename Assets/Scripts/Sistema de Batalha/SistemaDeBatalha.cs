using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.VirtualTexturing;
//using UnityEditor.TerrainTools;

public enum EstadoDeBatalha { Start, ActionSelection, MoveSelection, RunningTurn, Busy, PartyScreen, BattleOver }
public enum AcaoDeBatalha { Move, TrocarPokemon, Fugir }

public class SistemaDeBatalha : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI textoAcao;
    [SerializeField]  QuestController quest;
    [SerializeField]  GameController gm;
    //[SerializeField]  GameObject textPokebola;

    [Header("Dialogo")]
    [Space(15)]
    public DialogoDeBatalha dialogBox;

    [Header("Pokemons na Batalha")]
    [Space(15)]
    [SerializeField] Pokemon pokemonSelvagem;
    public BattleUnit playerUnit;
    public BattleUnit enemyUnit;
    public GameObject cenarioFloresta;
    public GameObject cenarioBrock;
    

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
    public bool isTrainerBattle = false;
    bool InimigoAtaca = true;

    [Header("Camera e Transicao")]
    [Space(15)]
    public GameObject batalha;
    [SerializeField] Caminhos caminhos;
    [SerializeField] LevelChanger Transitor;
    [SerializeField] LevelChanger Transitor2;

    //Outros
    public event Action<bool> OnBattleOver;
    [SerializeField] EstadoDeBatalha state;
    EstadoDeBatalha? prevState;
    [SerializeField] public bool batalhando = false;
    
    int currentAction; // acao atual
    public int currentMove; // acao atual
    int currentMember;
    
    //adicionei isso pra falar que, se capturou, ele não roda as falas
    bool capturou = false;

    public int EscapeAttempts { get; set; }


    public void Start()
    {
        pp = GameObject.Find("Player");
        player = pp.GetComponent<PlayerController>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log(pokemonSelvagem);
            Debug.Log(pokemonSelvagem.nome);
            
        }
    }

    public void AttPokebola()
    {
       // textPokebola.GetComponentInChildren<TextMeshProUGUI>().text = "x " + gm.pokeballs.ToString("n0");
    }
    public void StartBattle(PokemonParty playerParty, Pokemon pokemonSelvagem)
    {
        if (PlayerCanBattle)
        {
            AttPokebola();
            PlayerCanBattle = false;
            //Transitor = GetComponent<LevelChanger>();
            //StartCoroutine(Transitor.Transicao());
            //Transitor.TirarVel();

            PossoFugir = true;
            InimigoAtaca = true;
            batalhando = true;
            this.playerParty = playerParty;
            this.pokemonSelvagem = pokemonSelvagem;

            StartCoroutine(SetupBattle()); // Preparar para porrada
        }
        //else { ResetarTudo(); }
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
            treinador_atual = null;
            PlayerCanBattle = true;
            batalhando = false;


            //Transitor.DevolverVel();

            playerUnit.DestroyInstantiatedModel(); // sumindo com os modelos 3d do player
            enemyUnit.DestroyInstantiatedModel(); // sumindo com os modelos 3d do inimigo
        }
    
    }

    public IEnumerator SetupBattle()
    {
        
        playerUnit.Setup(playerParty.GetHealthyPokemon());
        enemyUnit.Setup(pokemonSelvagem);

        cenarioFloresta.SetActive(true);
        cenarioBrock.SetActive(false);

        playerUnit.Hud.SetarVida(playerUnit.Pokemon);

        partyScreen.Init();

        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);


        yield return dialogBox.TypeDialog($"Um {enemyUnit.Pokemon.Base.Nome} apareceu!");

        yield return new WaitForSeconds(1.5f);

        Debug.Log("ActionSelection!");
        ActionSelection();
        batalhando = true;
    }

    public Pokemon GetPokemonPlayer(Pokemon pkm)
    {

        if (pkm.nome == playerUnit.Pokemon.nome)
        {
            Debug.Log(playerUnit.Pokemon.HP);
            return playerUnit.Pokemon;
            
        }
        else
        {
            Debug.Log("Pkmns nao sao iguais");
            return null;
        }
        
    }

    public void StartTrainerBattle(PokemonParty playerParty, PokemonParty trainerParty)
    {
        if (PlayerCanBattle)
        {
            AttPokebola();
            PlayerCanBattle = false;
            //Transitor = GetComponent<LevelChanger>();
            //StartCoroutine(Transitor.Transicao());
            //Transitor.TirarVel();

            PossoFugir = true;
            this.playerParty = playerParty;
            this.trainerParty = trainerParty;

            isTrainerBattle = true;
            batalhando = true;


            StartCoroutine(SetupBattleTrainer()); // Preparar para porrada
        }
        //else { ResetarTudo(); }
        
    }
    

    public IEnumerator SetupBattleTrainer()
    {
        playerUnit.Setup(playerParty.GetHealthyPokemon());
        enemyUnit.Setup(trainerParty.GetHealthyPokemon());

        if (treinador_atual.name == "Brock")
        {
            cenarioFloresta.SetActive(false);
            cenarioBrock.SetActive(true);
        }
        else
        {
            cenarioFloresta.SetActive(true);
            cenarioBrock.SetActive(false);
        }

        playerUnit.Hud.SetarVida(playerUnit.Pokemon);

        partyScreen.Init();

        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);

        yield return dialogBox.TypeDialog($"Um {enemyUnit.Pokemon.Base.Nome} apareceu!");

        yield return new WaitForSeconds(1.5f);

        Debug.Log("ActionSelectionTrainerBattle!");
        ActionSelection();
        batalhando = true;
    }

    void BattleOver(bool won)
    {
        //StartCoroutine(Transitor2.Transicao());

        state = EstadoDeBatalha.BattleOver;
        playerParty.Pokemons.ForEach(p => p.OnBattleOver());
        StartCoroutine(TerminarCombate(won));
        playerUnit.DestroyInstantiatedModel();

    }

    public IEnumerator TerminarCombate(bool won)
    {
        dialogBox.AtivarSelecaoAcao(false);

        yield return new WaitForSeconds(1f);

        if (won == true && capturou) 
        { 
            dialogBox.SetDialog("Você venceu a batalha!");
            yield return new WaitForSeconds(0.5f);

        }
        else if (!won && capturou) { dialogBox.SetDialog("Você perdeu a batalha..."); }

        yield return new WaitForSeconds(1f);
        //Venci Batalha selvagem
        if (won == true && treinador_atual == null) 
        {
            UparPokemon(); 
            PlayerCanBattle = true;
        }

        //Venci batalha com treinador
        if (treinador_atual != null && won == true)
        {
            Debug.Log(treinador_atual);

            var _treinador = treinador_atual.GetComponent<TrainerController>();
            var _treinadorcharacter = treinador_atual.GetComponent<Character>();

            //Nao deixa esse treinador batalhar 
            _treinador.col.enabled = false;
            _treinador.PerdiBatalha = true;
            _treinador.posso_batalha = false;

            UparPokemon();

            //Removendo os scripts do treinador logo pra nao dar errado
            Destroy(_treinador);
            Destroy(_treinadorcharacter);

            //_parar.gameObject.SetActive(false);

            treinador_atual = null;
            //PlayerCanBattle = true;
        }

        dialogBox.SetDialog("");



        playerUnit.DestroyInstantiatedModel(); // sumindo com os modelos 3d do player
        enemyUnit.DestroyInstantiatedModel(); // sumindo com os modelos 3d do inimigo
        PossoFugir = true;
        //batalha.SetActive(false);


        #region TELEPORTE PÓS MORTE
        if (won == false && quest.centroPokemon == false)//Perdi e meus pokemons morreram = voltar pra casa
        {
            Transitor.Teleporte();
            foreach (Pokemon p in playerParty.pokemons)
            {
                p.HP = p.VidaMax;
            }
            PlayerCanBattle = true;
            Debug.Log("Rodei");
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

        OnBattleOver(won);
        ResetarTudo();
        isTrainerBattle = false;
    }

    public void ActionSelection()
    {
        state = EstadoDeBatalha.ActionSelection;
        
        dialogBox.AtivarSelecaoAcao(true);
       // StartCoroutine(dialogBox.TypeDialog("Escolha sua ação"));
        dialogBox.SetDialog("Escolha sua ação.");

        Debug.Log("Rodei ActionSelection!");
    }

    public IEnumerator ChecarEvolucao(Pokemon pkm)
    {
        if (pkm.level == pkm.LevelEvolucao)
        {
            Debug.Log("VOU EVOLUIR");
            playerParty.pokemons.Remove(pkm);

            var _pkm = pkm.Evolucao;
            _pkm.level = pkm.level;
            playerParty.pokemons.Add(_pkm);

            yield return new WaitForSeconds(0.5f);

            playerUnit.DestroyInstantiatedModel();
            _pkm.Init();
            if (state != EstadoDeBatalha.BattleOver)
            {
                //BOTA O MONSTRO PRA JOGO!!
                playerUnit.Setup(_pkm);
            }

            dialogBox.SetDialog($"Seu {pkm.nome} evoluiu para... {_pkm.nome}!");

            yield return new WaitForSeconds(2f);
        }
        else
        {
            //ooomaga
            Debug.Log("Nao chegou no nivel para evoluir");
        }
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
                    playerUnit.Hud.BarraXp.SetXP((float)p.XpAtual / Mathf.Pow(p.level, 3));

                    var CondicaoUp = Math.Pow(p.level, 3); //Quantia de xp pra upar:  level ao cubo

                    if (p.XpAtual >= CondicaoUp)
                    {
                        p.XpAtual = 0;
                        p.level++;
                        StartCoroutine(ChecarEvolucao(p));
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

    public void GanharPokemon()
    {
        //Checar se tenho espaço na party
        if (playerParty.Pokemons.Count <= 5)
        {
            playerParty.pokemons.Add(pokemonSelvagem);
            pokemonSelvagem.Init(); 
        }
        else //Vai mandar pro PC
        {
            gm.PC.Add(pokemonSelvagem);
            pokemonSelvagem.Init();
        }
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
                if (targetUnit.Pokemon == enemyUnit.Pokemon)
                {
                    dialogBox.SetDialog($"{targetUnit.Pokemon.Base.Nome} foi derrotado!");
                    yield return new WaitForSeconds(2f);
                    dialogBox.SetDialog($"Você ganhou {targetUnit.Pokemon.XpGiven} de experiência.");
                    yield return new WaitForSeconds(1f);
                    var _moedas = UnityEngine.Random.Range(2 * targetUnit.Pokemon.level, 10 * targetUnit.Pokemon.level);
                    gm.moedas += Mathf.RoundToInt(_moedas);
                    dialogBox.SetDialog($"Você ganhou {_moedas} moedas.");
                    //Reproduzir a animacao de desmaio aqui 
                    // ---------\\ 
                    yield return new WaitForSeconds(1f);

                    targetUnit.DestroyInstantiatedModel();
                    //sourceUnit.Hud.UpdateXP();

                    CheckForBattleOver(targetUnit);
                }
                else //Seu pokemon foi de base
                {
                    dialogBox.SetDialog($"Seu {targetUnit.Pokemon.Base.Nome} foi derrotado...");
                    //Reproduzir a animacao de desmaio aqui 
                    // ---------\\ 
                    yield return new WaitForSeconds(2f);

                    targetUnit.DestroyInstantiatedModel();
                    //sourceUnit.Hud.UpdateXP();

                    CheckForBattleOver(targetUnit);
                }

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

        int accuracy = source.StatBoosts[Stat.Precisão];
        int evasion = target.StatBoosts[Stat.Evasão];

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
        if (faintedUnit.IsPlayerUnit) //JOGADOR
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
        else if (faintedUnit.IsPlayerUnit == false && treinador_atual != null) //INIMIGO TREINADOR
        {
            var nextPokemon = treinador_atual.GetComponent<PokemonParty>().GetHealthyPokemon();

            if (nextPokemon != null)
            {
                enemyUnit.Setup(nextPokemon);
            }
            else
            {
                PlayerCanBattle = true;
                BattleOver(true);
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
            yield return dialogBox.TypeDialog("Não foi muito efetivo!");
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
        //currentMove = 0;
        currentMember = 0;
    }
    public void S2()
    {
        //currentMove = 1;
        currentMember = 1;
    }

    public void S3()
    {
        //currentMove = 2;
        currentMember = 2;
    }

    public void S4()
    {
        //currentMove = 3;
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
        ActionSelection();
        partyScreen.gameObject.SetActive(false);
        textoAcao.enabled = true;
    }

    public IEnumerator Esperar(float espera)
    {
        yield return new WaitForSeconds(espera);
    }

    public void SairMochila()
    {
        if (batalhando == true)
        {
            dialogBox.AtivarSelecaoMochila(false);
            dialogBox.AtivarSelecaoAcao(true);
            dialogBox.AtivarSelecaoMochilaUsar(false);
            gm.LimparInfoMochila();
            textoAcao.enabled = true;
        }
    }

    public void SairGolpe()
    {
        ActionSelection();
        dialogBox.AtivarSelecaoGolpe(false);
        textoAcao.enabled = true;
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

    public void Mochila()
    {
        if (batalhando == true)
        {
            dialogBox.AtivarSelecaoAcao(false);
            dialogBox.AtivarSelecaoMochila(true);
        }

        var _p = gm.mochila.GetComponent<PcInfoChanger>();

        for (int i = 0; i < _p.slotsParty.Length; i++)
        {
            _p.UpdateSlotParty(i);
        }

        //StartCoroutine(TryToCatch());
    }

    public IEnumerator TurnoInimigo()
    {
        yield return new WaitForSeconds(2f);


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

    public IEnumerator TryToCatch()
    {
        //Nao da pra capturar do treinador
        if (isTrainerBattle)
        {
            yield return dialogBox.TypeDialog($"Você não pode capturar pokemons de um treinador!");

            StartCoroutine(TurnoInimigo());
        }
        else
        {
            //Entao perde uma pokebola
            //gm.pokeballs--;

                var ConseguiCapturar = gm.item_atual.Captura(pokemonSelvagem);

                if (ConseguiCapturar)
                {
                    
                    dialogBox.SetDialog($"{pokemonSelvagem.nome} foi capturado!");
                    yield return new WaitForSeconds(2f);
                    capturou = true;

                /* bs.*/

                    playerUnit.DestroyInstantiatedModel(); // sumindo com os modelos 3d do player
                    enemyUnit.DestroyInstantiatedModel(); // sumindo com os modelos 3d do inimigo

                    BattleOver(true);

                    GanharPokemon();
                    
                }
                else if (!ConseguiCapturar)
                {

                    dialogBox.SetDialog($"Não conseguiu capturar...");
                    yield return new WaitForSeconds(2f);

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
                    textoAcao.enabled = true;
                   
                    yield break;

                }
                else
                {
                    // Trate a situação onde item_atual é nulo (talvez exiba uma mensagem de erro ou tome alguma ação padrão)
                    Debug.LogError("item_atual é nulo.");
                }
               
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
