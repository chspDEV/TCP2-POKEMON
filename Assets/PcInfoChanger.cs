using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class PcInfoChanger : MonoBehaviour
{
    public GameController gm;
    public PlayerController player;

    public TextMeshProUGUI nome;
    public TextMeshProUGUI lvl; //Lv. level
    public TextMeshProUGUI hp;

    public GameObject adicionar;
    public GameObject remover;
    public Pokemon pkm_selecionado;

    public TextMeshProUGUI[] golpes; //NomeGolpe (Tipo)

    public GameObject[] slotsParty;
    
    public GameObject[] slotsPC;
    public Sprite bgPCslot;

    public void UpdateTextInfo(Pokemon pkm)
    {
        if (pkm.nome != "PlaceHolder")
        {
            nome.text = pkm.nome;
            lvl.text = $"Lv. {pkm.level}";
            hp.text = $"{pkm.HP}/{pkm.VidaMax}";

            for (int i = 0; i < pkm.Moves.Count; i++)
            {
                golpes[i].text = $"{i + 1}. {pkm.Moves[i].Base.Nome} ({pkm.Moves[i].Base.Tipo})";
            }
        }
    }

    public void DeleteTextInfo()
    {
        nome.text = "";
        lvl.text = "";
        hp.text = $"";

        for (int i = 0; i < golpes.Length; i++)
        {
            golpes[i].text = $"{i+1}. ";
        }
    }

    public void UpdateSlotPC(int _index)
    {
        if (_index < gm.PC.Count && gm.PC[_index].nome != "PlaceHolder")
        {
            //Iniciando o pokemon!
            gm.PC[_index].Init();

            //Atualizando imagem do pokemon no PC
            slotsPC[_index].GetComponent<SlotPKMPC>().minhaImagem.sprite = gm.PC[_index].sprite;
            var imgColor = slotsPC[_index].GetComponent<SlotPKMPC>().minhaImagem.color;
            slotsPC[_index].GetComponent<SlotPKMPC>().minhaImagem.color = new Color(imgColor.r, imgColor.g, imgColor.b, 1f);

            //Atualizando nome e nivel
            slotsPC[_index].GetComponentInChildren<TextMeshProUGUI>().text = gm.PC[_index].nome + "\n Lv. " + gm.PC[_index].level.ToString("n0");
        }
        else
        {
            //Se não existe pokemon nesse index, deixe tudo em branco!
            slotsPC[_index].GetComponent<SlotPKMPC>().minhaImagem.sprite = null;
            slotsPC[_index].GetComponent<SlotPKMPC>().minhaImagem.color = new Color(1, 1, 1, 0f);
            slotsPC[_index].GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
        
    }

    public void UpdateSlotParty(int _index)
    {
        PokemonParty _pt = player.GetComponent<PokemonParty>();

        if (_index < _pt.pokemons.Count && _pt.pokemons[_index].nome != "PlaceHolder")
        {
            //Iniciando o pokemon!
            _pt.pokemons[_index].Init();

            //Atualizando imagem do pokemon no PC
            slotsParty[_index].GetComponent<SlotPKM>().minhaImagem.sprite = _pt.pokemons[_index].sprite;
            var imgColor = slotsParty[_index].GetComponent<SlotPKM>().minhaImagem.color;
            slotsParty[_index].GetComponent<SlotPKM>().minhaImagem.color = new Color(imgColor.r, imgColor.g, imgColor.b, 1f);

            //Atualizando nome e nivel
            slotsParty[_index].GetComponentInChildren<TextMeshProUGUI>().text = _pt.pokemons[_index].nome + "\n Lv. " + _pt.pokemons[_index].level.ToString("n0");
        }
        else
        {
            //Se não existe pokemon nesse index, deixe tudo em branco!
            slotsParty[_index].GetComponent<SlotPKM>().minhaImagem.sprite = null;
            slotsParty[_index].GetComponent<SlotPKM>().minhaImagem.color = new Color(1, 1, 1, 0f);
            slotsParty[_index].GetComponentInChildren<TextMeshProUGUI>().text = "";
        }

    }

    public Pokemon DescobrirPkmParty(int _index)
    {
        PokemonParty _pt = player.GetComponent<PokemonParty>();

        if (_index < _pt.pokemons.Count) { return _pt.pokemons[_index]; }
        else { return null; }
    }

    public Pokemon DescobrirPkmPC(int _index)
    {
        if (_index < gm.PC.Count) { return gm.PC[_index]; }
        else { return null; }
    }

    public void ControlarBotao(bool _adicionar, bool _remover)
    {
        adicionar.SetActive(_adicionar);
        remover.SetActive(_remover);
    }

    public void AdicionarNaParty()
    {
        PokemonParty _pt = player.GetComponent<PokemonParty>();
        _pt.pokemons.Add(pkm_selecionado);
        gm.PC.Remove(pkm_selecionado);

        pkm_selecionado = null;
        ControlarBotao(false, false);

    }

    public void RemoverDaParty()
    {
        PokemonParty _pt = player.GetComponent<PokemonParty>();
        _pt.pokemons.Remove(pkm_selecionado);
        gm.PC.Add(pkm_selecionado);

        pkm_selecionado = null;
        ControlarBotao(false, false);
    }


}
