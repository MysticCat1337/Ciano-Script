using UnityEngine;
using System.Collections.Generic;

public class CianoPlayerScript : MonoBehaviour
{
    Transform raizMeshes;
    RuntimeModVariables runtimeVars;

    float tempoFolha1Abrir = 2.10f;
    float tempoFolha2Abrir = 2.10f;
    float tempoFolha2Fechar = 1.80f;
    float tempoFolha1Fechar = 1.80f;

    float inicioFolha2AbrirPorta1 = 0.85f;
    float inicioFolha2AbrirPorta2 = 0.60f;
    float inicioFolha2AbrirPorta3 = 0.75f;

    float inicioFolha1FecharPorta1 = 0.85f;
    float inicioFolha1FecharPorta2 = 0.60f;
    float inicioFolha1FecharPorta3 = 0.75f;

    // ============================================================
    // CONFIGURAÇÃO DA BATIDA DAS PORTAS
    // ============================================================

    int intensidadeBatidaPorta = 2;
    float duracaoBatidaPorta = 0.22f;
    float frequenciaBatidaPorta = 30f;
    float inicioBatidaPorta = 0.04f;

    class Porta
    {
        public int id;
        public MEPCadaPortaBus door;
        public Transform folha1;
        public Transform folha2;

        public Quaternion rotacaoFechada1;
        public Quaternion rotacaoFechada2;
        public Quaternion rotacaoAberta1;
        public Quaternion rotacaoAberta2;

        public float inicioFolha2Abrir;
        public float inicioFolha1Fechar;
        public float tempo;
        public bool estadoAnterior;

        public float progressoFolha1;
        public float progressoFolha2;

        public bool batidaFolha1Ativa;
        public bool batidaFolha1Executada;
        public float tempoBatidaFolha1;

        public bool batidaFolha2Ativa;
        public bool batidaFolha2Executada;
        public float tempoBatidaFolha2;

        public Porta(int id)
        {
            this.id = id;
            tempo = 0f;
            estadoAnterior = false;

            progressoFolha1 = 0f;
            progressoFolha2 = 0f;

            batidaFolha1Ativa = false;
            batidaFolha1Executada = true;
            tempoBatidaFolha1 = 0f;

            batidaFolha2Ativa = false;
            batidaFolha2Executada = true;
            tempoBatidaFolha2 = 0f;
        }
    }

    List<Porta> portas = new List<Porta>();

    class AnimacaoPivo
    {
        public string marcador;
        public Transform pivo;
        public List<Transform> objetos = new List<Transform>();
        public Vector3 eixo;
        public float angulo;
        public float velocidade;
        public bool estado;
        public float anguloAtual;
    }

    AnimacaoPivo capo;
    AnimacaoPivo capolet;
    AnimacaoPivo sunshade;
    AnimacaoPivo parkingBrake;
    AnimacaoPivo tampa1;
    AnimacaoPivo tampa2;

    // ============================================================
    // JANELAS DO MOTORISTA
    // ============================================================

    class AnimacaoDeslizante
    {
        public string marcador;
        public Transform pivo;
        public List<Transform> objetos = new List<Transform>();

        public Vector3 direcao;

        public float distancia;
        public float distanciaLocal;

        public float duracao;

        public bool estado;
        public float progresso;

        public Vector3 posicaoFechada;
    }

    AnimacaoDeslizante janelaMotor1;
    AnimacaoDeslizante janelaMotor2;

    AnimacaoPivo janelaMotor3;

    float distanciaAberturaJanelaMotor = 0.21f;
    float distanciaAberturaJanelaMotor2 = 0.07f;

    float duracaoAnimacaoJanelaMotor = 0.70f;

    // ============================================================
    // CÂMBIO
    // ============================================================

    Transform cambio;
    Transform pivoCambio;

    // ============================================================
    // PIVÔ DO VENTILADOR
    // ============================================================

    Transform pivoVentilador;

    Vector3 posicaoPivoVentilador =
        new Vector3(
            -1.22319f,
            2.51415f,
            5.5981f
        );

    Quaternion cambioN;
    Quaternion cambio1;
    Quaternion cambio2;
    Quaternion cambio3;
    Quaternion cambio4;
    Quaternion cambio5;
    Quaternion cambio6;
    Quaternion cambioR;

    Quaternion cambioRotacaoInicio;
    Quaternion cambioAlvoAtual;
    Quaternion rotacaoBaseCambio;

    int marchaCambioAnterior = 999;

    float tempoAnimacaoCambio = 0f;
    float duracaoCambio = 0.45f;

    // ============================================================
    // VENTILADOR
    // ============================================================

    Transform[] ventiladores =
        new Transform[9];

    Renderer[][] renderersVentilador =
        new Renderer[9][];

    Transform carcaçaVentilador;

    float velocidadeVentilador = 18f;
    float tempoVentilador = 0f;
    int frameVentilador = -1;

    // ============================================================
    // VIBRAÇÃO
    // ============================================================

    float tempoVibracao = 0f;

    float intensidadeVibracaoCambio = 0.45f;
    float velocidadeVibracaoCambio = 58f;

    float intensidadeVibracaoNormal = 1.0f;

    float intensidadeVibracaoPartida = 1.80f;

    float duracaoVibracaoPartida = 0.80f;

    float duracaoVibracaoDesligamento = 0.70f;

    float duracaoTransicaoAceleracao = 0.80f;

    float duracaoTransicaoParado = 0.60f;

    float velocidadeParado = 0.15f;

    float velocidadeSemVibracao = 2.0f;

    float velocidadeAtual = 0f;
    float velocidadeAnterior = 0f;

    float nivelVibracao = 0f;

    bool motorAnterior = false;

    float tempoPartidaMotor = 999f;

    float tempoDesligamentoMotor = 999f;

    Vector3 posicaoAnteriorOnibus;

    bool posicaoInicialRegistrada = false;

    // ============================================================
    // RETROVISORES
    // ============================================================

    Transform pivoRetrovisorEsquerdo;
    Transform pivoRetrovisorDireito;

    List<Transform> objetosRetrovisorEsquerdo =
        new List<Transform>();

    List<Transform> objetosRetrovisorDireito =
        new List<Transform>();

    float anguloRetrovisorEsquerdo = 0f;
    float anguloRetrovisorDireito = 0f;

    float limiteRotacaoRetrovisor = 5f;

    float grausPorPixelRetrovisor = 0.05f;

    bool arrastandoRetrovisorEsquerdo = false;
    bool arrastandoRetrovisorDireito = false;

    float ultimaPosicaoMouseX = 0f;

    // ============================================================
    // CATRAÇA
    // ============================================================

    Transform pivoCatraca;

    List<Transform> objetosCatraca =
        new List<Transform>();

    float anguloCatraca = 0f;

    float anguloAlvoCatraca = 0f;

    float incrementoCatraca = -90f;

    float velocidadeCatraca = 180f;

    bool catracaAnimando = false;

    Vector3 posicaoPivoCatraca =
        new Vector3(
            0.06441f,
            1.26328f,
            2.97254f
        );

    // ============================================================
    // START
    // ============================================================

    void Start()
    {
        raizMeshes =
            CianoUtils.GetPlayerRootMeshes();

        runtimeVars =
            CianoUtils.GetRuntimeModVariables();

        PrepararPorta(1);
        PrepararPorta(2);
        PrepararPorta(3);

        capo = CriarAnimacaoPivo(
            "_capo_",
            new Vector3(
                -0.004052f,
                1.36411f,
                5.98964f
            ),
            Vector3.right,
            -70.8f
        );

        capolet = CriarAnimacaoPivo(
            "_capolet_",
            new Vector3(
                0f,
                3.12237f,
                5.59861f
            ),
            Vector3.right,
            82.1f
        );

        sunshade = CriarAnimacaoPivo(
            "_sunshade_",
            new Vector3(
                -0.710587f,
                2.53756f,
                5.60634f
            ),
            Vector3.right,
            106f
        );

        parkingBrake = CriarAnimacaoPivo(
            "_parking_brake_",
            new Vector3(
                -1.09237f,
                1.95032f,
                5.41791f
            ),
            Vector3.right,
            75.9f
        );

        tampa1 = CriarAnimacaoPivo(
            "tampa1",
            new Vector3(
                -1.2694f,
                1.04956f,
                5.07693f
            ),
            Vector3.forward,
            -90f
        );

        tampa2 = CriarAnimacaoPivo(
            "tampa2",
            new Vector3(
                -1.27366f,
                1.05883f,
                2.25628f
            ),
            Vector3.forward,
            -90f
        );

        // ========================================================
        // PREPARAR JANELAS DO MOTORISTA
        // ========================================================

        PrepararJanelasMotorista();

        PrepararCambio();
        PrepararVentilador();

        // ========================================================
        // PREPARAR RETROVISORES
        // ========================================================

        PrepararRetrovisores();

        // ========================================================
        // PREPARAR CATRAÇA
        // ========================================================

        PrepararCatraca();

        if (raizMeshes != null)
        {
            posicaoAnteriorOnibus =
                raizMeshes.position;

            posicaoInicialRegistrada =
                true;
        }

        if (runtimeVars != null)
        {
            motorAnterior =
                runtimeVars.engineOn;
        }

        // ========================================================
        // MOBITEC
        // ========================================================

        IniciarMobitec();

        Debug.Log(
            "Ciano: sistema combinado de animações carregado."
        );
    }

    // ============================================================
    // CRIAR ANIMAÇÃO COM PIVÔ
    // ============================================================

    AnimacaoPivo CriarAnimacaoPivo(
        string marcador,
        Vector3 posicao,
        Vector3 eixo,
        float angulo
    )
    {
        AnimacaoPivo anim =
            new AnimacaoPivo();

        anim.marcador = marcador;
        anim.eixo = eixo.normalized;
        anim.angulo = angulo;
        anim.velocidade = 90f;
        anim.estado = false;
        anim.anguloAtual = 0f;

        if (raizMeshes == null)
            return anim;

        Transform[] todos =
            raizMeshes.GetComponentsInChildren<Transform>(
                true
            );

        foreach (Transform t in todos)
        {
            if (t.name.Contains(marcador))
            {
                anim.objetos.Add(t);

                CianoUtils.CreateMeshColliderForButton(
                    t.gameObject
                );
            }
        }

        if (anim.objetos.Count == 0)
        {
            Debug.Log(
                "Ciano: nenhum objeto encontrado para " +
                marcador
            );

            return anim;
        }

        GameObject objetoPivo =
            new GameObject(
                "CianoPivo" + marcador
            );

        anim.pivo =
            objetoPivo.transform;

        anim.pivo.SetParent(
            raizMeshes,
            false
        );

        anim.pivo.localPosition =
            posicao;

        anim.pivo.localRotation =
            Quaternion.identity;

        foreach (Transform objeto in anim.objetos)
        {
            Vector3 posicaoMundo =
                objeto.position;

            Quaternion rotacaoMundo =
                objeto.rotation;

            objeto.SetParent(
                anim.pivo,
                true
            );

            objeto.position =
                posicaoMundo;

            objeto.rotation =
                rotacaoMundo;
        }

        return anim;
    }

    // ============================================================
    // CRIAR ANIMAÇÃO DESLIZANTE
    // ============================================================

    AnimacaoDeslizante CriarAnimacaoDeslizante(
        string marcador,
        Vector3 direcao,
        float distancia
    )
    {
        AnimacaoDeslizante anim =
            new AnimacaoDeslizante();

        anim.marcador =
            marcador;

        anim.direcao =
            direcao.normalized;

        anim.distancia =
            distancia;

        anim.duracao =
            duracaoAnimacaoJanelaMotor;

        anim.estado =
            false;

        anim.progresso =
            0f;

        if (raizMeshes == null)
            return anim;

        Transform[] todos =
            raizMeshes.GetComponentsInChildren<Transform>(
                true
            );

        foreach (Transform t in todos)
        {
            if (t.name.Contains(marcador))
            {
                anim.objetos.Add(t);

                CianoUtils.CreateMeshColliderForButton(
                    t.gameObject
                );
            }
        }

        if (anim.objetos.Count == 0)
        {
            Debug.Log(
                "Ciano: nenhum objeto encontrado para " +
                marcador
            );

            return anim;
        }

        GameObject objetoPivo =
            new GameObject(
                "CianoPivo" + marcador
            );

        anim.pivo =
            objetoPivo.transform;

        anim.pivo.SetParent(
            raizMeshes,
            false
        );

        anim.pivo.localPosition =
            Vector3.zero;

        anim.pivo.localRotation =
            Quaternion.identity;

        foreach (Transform objeto in anim.objetos)
        {
            Vector3 posicaoMundo =
                objeto.position;

            Quaternion rotacaoMundo =
                objeto.rotation;

            objeto.SetParent(
                anim.pivo,
                true
            );

            objeto.position =
                posicaoMundo;

            objeto.rotation =
                rotacaoMundo;
        }

        anim.posicaoFechada =
            anim.pivo.localPosition;

        float escalaZ =
            Mathf.Abs(
                raizMeshes.lossyScale.z
            );

        if (escalaZ < 0.0001f)
            escalaZ = 1f;

        anim.distanciaLocal =
            distancia /
            escalaZ;

        return anim;
    }

    // ============================================================
    // PREPARAR JANELAS DO MOTORISTA
    // ============================================================

    void PrepararJanelasMotorista()
    {
        janelaMotor1 =
            CriarAnimacaoDeslizante(
                "_abrevidromot1_",
                Vector3.back,
                distanciaAberturaJanelaMotor
            );

        janelaMotor2 =
            CriarAnimacaoDeslizante(
                "_abrevidromot2_",
                Vector3.forward,
                distanciaAberturaJanelaMotor2
            );

        janelaMotor3 =
            CriarAnimacaoPivo(
                "_abrevidromot3_",
                new Vector3(
                    -1.25692f,
                    2.20452f,
                    5.28627f
                ),
                Vector3.up,
                -14.3f
            );

        Debug.Log(
            "Ciano: janelas do motorista preparadas."
        );
    }

    // ============================================================
    // ATUALIZAR JANELAS DO MOTORISTA
    // ============================================================

    void AtualizarJanelasMotorista()
    {
        AtualizarAnimacaoDeslizante(
            janelaMotor1
        );

        AtualizarAnimacaoDeslizante(
            janelaMotor2
        );

        AtualizarAnimacaoPivo(
            janelaMotor3
        );
    }

    // ============================================================
    // ATUALIZAR ANIMAÇÃO DESLIZANTE
    // ============================================================

    void AtualizarAnimacaoDeslizante(
        AnimacaoDeslizante anim
    )
    {
        if (
            anim == null ||
            anim.pivo == null
        )
            return;

        float alvo =
            anim.estado
                ? 1f
                : 0f;

        if (anim.duracao <= 0.001f)
        {
            anim.progresso =
                alvo;
        }
        else
        {
            anim.progresso =
                Mathf.MoveTowards(
                    anim.progresso,
                    alvo,
                    Time.deltaTime /
                    anim.duracao
                );
        }

        float progressoSuave =
            Suavizar(
                anim.progresso
            );

        anim.pivo.localPosition =
            anim.posicaoFechada +
            anim.direcao *
            anim.distanciaLocal *
            progressoSuave;
    }

    // ============================================================
    // VERIFICAR CLIQUE DA JANELA DESLIZANTE
    // ============================================================

    void VerificarCliqueDeslizante(
        Transform atingido,
        AnimacaoDeslizante anim
    )
    {
        if (anim == null)
            return;

        foreach (Transform objeto in anim.objetos)
        {
            if (
                atingido == objeto ||
                atingido.IsChildOf(objeto)
            )
            {
                anim.estado =
                    !anim.estado;

                return;
            }
        }
    }

    // ============================================================
    // PREPARAR RETROVISORES
    // ============================================================

    void PrepararRetrovisores()
    {
        if (raizMeshes == null)
            return;

        Transform[] todos =
            raizMeshes.GetComponentsInChildren<Transform>(
                true
            );

        GameObject objetoPivoEsquerdo =
            new GameObject(
                "CianoPivo_RetrovisorEsquerdo"
            );

        pivoRetrovisorEsquerdo =
            objetoPivoEsquerdo.transform;

        pivoRetrovisorEsquerdo.SetParent(
            raizMeshes,
            false
        );

        pivoRetrovisorEsquerdo.localPosition =
            new Vector3(
                -1.44756f,
                2.22838f,
                5.46452f
            );

        pivoRetrovisorEsquerdo.localRotation =
            Quaternion.identity;

        GameObject objetoPivoDireito =
            new GameObject(
                "CianoPivo_RetrovisorDireito"
            );

        pivoRetrovisorDireito =
            objetoPivoDireito.transform;

        pivoRetrovisorDireito.SetParent(
            raizMeshes,
            false
        );

        pivoRetrovisorDireito.localPosition =
            new Vector3(
                1.3823f,
                2.30119f,
                5.98537f
            );

        pivoRetrovisorDireito.localRotation =
            Quaternion.identity;

        foreach (Transform t in todos)
        {
            if (
                t == pivoRetrovisorEsquerdo ||
                t == pivoRetrovisorDireito
            )
                continue;

            if (t.name.Contains("_retesq_"))
            {
                objetosRetrovisorEsquerdo.Add(t);
            }
            else if (t.name.Contains("_retdir_"))
            {
                objetosRetrovisorDireito.Add(t);
            }
        }

        foreach (
            Transform objeto
            in objetosRetrovisorEsquerdo
        )
        {
            Vector3 posicaoMundo =
                objeto.position;

            Quaternion rotacaoMundo =
                objeto.rotation;

            objeto.SetParent(
                pivoRetrovisorEsquerdo,
                true
            );

            objeto.position =
                posicaoMundo;

            objeto.rotation =
                rotacaoMundo;

            CianoUtils.CreateMeshColliderForButton(
                objeto.gameObject
            );
        }

        foreach (
            Transform objeto
            in objetosRetrovisorDireito
        )
        {
            Vector3 posicaoMundo =
                objeto.position;

            Quaternion rotacaoMundo =
                objeto.rotation;

            objeto.SetParent(
                pivoRetrovisorDireito,
                true
            );

            objeto.position =
                posicaoMundo;

            objeto.rotation =
                rotacaoMundo;

            CianoUtils.CreateMeshColliderForButton(
                objeto.gameObject
            );
        }

        Debug.Log(
            "Ciano: retrovisores preparados. " +
            "Esquerdo: " +
            objetosRetrovisorEsquerdo.Count +
            " objetos. Direito: " +
            objetosRetrovisorDireito.Count +
            " objetos."
        );
    }

    // ============================================================
    // ATUALIZAR RETROVISORES
    // ============================================================

    void AtualizarRetrovisores()
    {
        if (!CianoInput.GameInputAllowed())
        {
            arrastandoRetrovisorEsquerdo = false;
            arrastandoRetrovisorDireito = false;
            return;
        }

        if (!CianoInput.IsHoldingCtrl())
        {
            arrastandoRetrovisorEsquerdo = false;
            arrastandoRetrovisorDireito = false;
            return;
        }

        if (CianoInput.GetMouseButtonDown(0))
        {
            Camera camera =
                Camera.main;

            if (camera == null)
                return;

            Ray ray =
                camera.ScreenPointToRay(
                    Input.mousePosition
                );

            RaycastHit hit;

            if (
                Physics.Raycast(
                    ray,
                    out hit,
                    1000f,
                    1 << 13
                )
            )
            {
                Transform atingido =
                    hit.transform;

                if (
                    PertenceAoRetrovisor(
                        atingido,
                        objetosRetrovisorEsquerdo
                    )
                )
                {
                    arrastandoRetrovisorEsquerdo =
                        true;

                    arrastandoRetrovisorDireito =
                        false;

                    ultimaPosicaoMouseX =
                        Input.mousePosition.x;

                    return;
                }

                if (
                    PertenceAoRetrovisor(
                        atingido,
                        objetosRetrovisorDireito
                    )
                )
                {
                    arrastandoRetrovisorDireito =
                        true;

                    arrastandoRetrovisorEsquerdo =
                        false;

                    ultimaPosicaoMouseX =
                        Input.mousePosition.x;

                    return;
                }
            }
        }

        if (!Input.GetMouseButton(0))
        {
            arrastandoRetrovisorEsquerdo = false;
            arrastandoRetrovisorDireito = false;
            return;
        }

        if (arrastandoRetrovisorEsquerdo)
        {
            float mouseX =
                Input.mousePosition.x;

            float deltaX =
                mouseX -
                ultimaPosicaoMouseX;

            ultimaPosicaoMouseX =
                mouseX;

            anguloRetrovisorEsquerdo +=
                deltaX *
                grausPorPixelRetrovisor;

            anguloRetrovisorEsquerdo =
                Mathf.Clamp(
                    anguloRetrovisorEsquerdo,
                    -limiteRotacaoRetrovisor,
                    limiteRotacaoRetrovisor
                );

            AplicarRotacaoRetrovisorEsquerdo();
        }

        if (arrastandoRetrovisorDireito)
        {
            float mouseX =
                Input.mousePosition.x;

            float deltaX =
                mouseX -
                ultimaPosicaoMouseX;

            ultimaPosicaoMouseX =
                mouseX;

            anguloRetrovisorDireito +=
                deltaX *
                grausPorPixelRetrovisor;

            anguloRetrovisorDireito =
                Mathf.Clamp(
                    anguloRetrovisorDireito,
                    -limiteRotacaoRetrovisor,
                    limiteRotacaoRetrovisor
                );

            AplicarRotacaoRetrovisorDireito();
        }
    }

    // ============================================================
    // VERIFICAR OBJETO DO RETROVISOR
    // ============================================================

    bool PertenceAoRetrovisor(
        Transform atingido,
        List<Transform> objetos
    )
    {
        foreach (Transform objeto in objetos)
        {
            if (
                atingido == objeto ||
                atingido.IsChildOf(objeto)
            )
            {
                return true;
            }
        }

        return false;
    }

    // ============================================================
    // APLICAR ROTAÇÃO DO RETROVISOR ESQUERDO
    // ============================================================

    void AplicarRotacaoRetrovisorEsquerdo()
    {
        if (pivoRetrovisorEsquerdo == null)
            return;

        pivoRetrovisorEsquerdo.localRotation =
            Quaternion.AngleAxis(
                anguloRetrovisorEsquerdo,
                Vector3.up
            );
    }

    // ============================================================
    // APLICAR ROTAÇÃO DO RETROVISOR DIREITO
    // ============================================================

    void AplicarRotacaoRetrovisorDireito()
    {
        if (pivoRetrovisorDireito == null)
            return;

        pivoRetrovisorDireito.localRotation =
            Quaternion.AngleAxis(
                anguloRetrovisorDireito,
                Vector3.up
            );
    }

    // ============================================================
    // CLIQUES
    // ============================================================

    void AtualizarCliques()
    {
        if (!CianoInput.GameInputAllowed())
            return;

        if (!CianoInput.IsHoldingCtrl())
            return;

        if (!CianoInput.GetMouseButtonDown(0))
            return;

        Camera camera =
            Camera.main;

        if (camera == null)
            return;

        Ray ray =
            camera.ScreenPointToRay(
                Input.mousePosition
            );

        RaycastHit hit;

        if (
            Physics.Raycast(
                ray,
                out hit,
                1000f,
                1 << 13
            )
        )
        {
            Transform atingido =
                hit.transform;

            VerificarClique(atingido, capo);
            VerificarClique(atingido, capolet);
            VerificarClique(atingido, sunshade);
            VerificarClique(atingido, tampa1);
            VerificarClique(atingido, tampa2);

            VerificarCliqueDeslizante(
                atingido,
                janelaMotor1
            );

            VerificarCliqueDeslizante(
                atingido,
                janelaMotor2
            );

            VerificarClique(
                atingido,
                janelaMotor3
            );
        }
    }

    void VerificarClique(
        Transform atingido,
        AnimacaoPivo anim
    )
    {
        if (anim == null)
            return;

        foreach (Transform objeto in anim.objetos)
        {
            if (
                atingido == objeto ||
                atingido.IsChildOf(objeto)
            )
            {
                anim.estado =
                    !anim.estado;

                return;
            }
        }
    }

    // ============================================================
    // ANIMAR PIVÔ
    // ============================================================

    void AtualizarAnimacaoPivo(
        AnimacaoPivo anim
    )
    {
        if (
            anim == null ||
            anim.pivo == null
        )
            return;

        float alvo =
            anim.estado
                ? anim.angulo
                : 0f;

        anim.anguloAtual =
            Mathf.MoveTowards(
                anim.anguloAtual,
                alvo,
                anim.velocidade *
                Time.deltaTime
            );

        anim.pivo.localRotation =
            Quaternion.AngleAxis(
                anim.anguloAtual,
                anim.eixo
            );
    }

    // ============================================================
    // PREPARAR CÂMBIO
    // ============================================================

    void PrepararCambio()
    {
        if (raizMeshes == null)
            return;

        Transform[] todos =
            raizMeshes.GetComponentsInChildren<Transform>(
                true
            );

        foreach (Transform t in todos)
        {
            if (t.name.Contains("_cambio_"))
            {
                cambio = t;
                break;
            }
        }

        if (cambio == null)
        {
            Debug.Log(
                "Ciano: _cambio_ não encontrado."
            );

            return;
        }

        GameObject objetoPivo =
            new GameObject(
                "CianoPivo_Cambio"
            );

        pivoCambio =
            objetoPivo.transform;

        pivoCambio.SetParent(
            raizMeshes,
            false
        );

        pivoCambio.localPosition =
            new Vector3(
                -0.365553f,
                1.51101f,
                5.01659f
            );

        pivoCambio.localRotation =
            Quaternion.identity;

        Vector3 posicaoMundo =
            cambio.position;

        Quaternion rotacaoMundo =
            cambio.rotation;

        cambio.SetParent(
            pivoCambio,
            true
        );

        cambio.position =
            posicaoMundo;

        cambio.rotation =
            rotacaoMundo;

        cambioN =
            Quaternion.Euler(
                0f,
                0f,
                0f
            );

        cambio1 =
            Quaternion.Euler(
                23.0995f,
                -8.99045f,
                0.617581f
            );

        cambio2 =
            Quaternion.Euler(
                -8.00621f,
                13.1189f,
                3.91936f
            );

        cambio3 =
            Quaternion.Euler(
                28.1887f,
                0f,
                0f
            );

        cambio4 =
            Quaternion.Euler(
                -11.5332f,
                0f,
                0f
            );

        cambio5 =
            Quaternion.Euler(
                15.7585f,
                13.5847f,
                -1.64433f
            );

        cambio6 =
            Quaternion.Euler(
                -9.26935f,
                -7.93998f,
                -4.27555f
            );

        cambioR =
            Quaternion.Euler(
                -8.52387f,
                20.2263f,
                4.06817f
            );

        cambioAlvoAtual =
            cambioN;

        cambioRotacaoInicio =
            cambioN;

        rotacaoBaseCambio =
            cambioN;

        tempoAnimacaoCambio =
            1f;

        marchaCambioAnterior =
            0;

        CianoUtils.CreateMeshColliderForButton(
            cambio.gameObject
        );

        Debug.Log(
            "Ciano: câmbio preparado. 1ª e 5ª foram trocadas."
        );
    }

    // ============================================================
    // OBTER ROTAÇÃO DA MARCHA
    // ============================================================

    Quaternion ObterRotacaoCambio(
        int marcha
    )
    {
        switch (marcha)
        {
            case -1:
                return cambioR;

            case 0:
                return cambioN;

            case 1:
                return cambio1;

            case 2:
                return cambio2;

            case 3:
                return cambio3;

            case 4:
                return cambio4;

            case 5:
                return cambio5;

            case 6:
                return cambio6;

            default:
                return cambioN;
        }
    }

    // ============================================================
    // ATUALIZAR CÂMBIO
    // ============================================================

    void AtualizarCambio()
    {
        if (
            pivoCambio == null ||
            runtimeVars == null
        )
            return;

        int marcha;

        if (runtimeVars.isManualGearbox)
        {
            marcha =
                runtimeVars.manualGear;
        }
        else
        {
            marcha =
                runtimeVars.automaticGear;
        }

        if (marcha != marchaCambioAnterior)
        {
            cambioRotacaoInicio =
                rotacaoBaseCambio;

            cambioAlvoAtual =
                ObterRotacaoCambio(marcha);

            tempoAnimacaoCambio =
                0f;

            marchaCambioAnterior =
                marcha;
        }

        tempoAnimacaoCambio +=
            Time.deltaTime;

        float progresso =
            Mathf.Clamp01(
                tempoAnimacaoCambio /
                duracaoCambio
            );

        float progressoSuave =
            progresso *
            progresso *
            (3f - 2f * progresso);

        rotacaoBaseCambio =
            Quaternion.Slerp(
                cambioRotacaoInicio,
                cambioAlvoAtual,
                progressoSuave
            );

        pivoCambio.localRotation =
            rotacaoBaseCambio;
    }

    // ============================================================
    // PREPARAR VENTILADOR
    // ============================================================

    void PrepararVentilador()
    {
        if (raizMeshes == null)
            return;

        Transform[] todos =
            raizMeshes.GetComponentsInChildren<Transform>(
                true
            );

        GameObject objetoPivo =
            new GameObject(
                "CianoPivo_Ventilador"
            );

        pivoVentilador =
            objetoPivo.transform;

        pivoVentilador.SetParent(
            raizMeshes,
            false
        );

        pivoVentilador.localPosition =
            posicaoPivoVentilador;

        pivoVentilador.localRotation =
            Quaternion.identity;

        foreach (Transform t in todos)
        {
            if (t.name == "zzzextraventiladorON")
            {
                carcaçaVentilador =
                    t;

                Vector3 posicaoMundo =
                    t.position;

                Quaternion rotacaoMundo =
                    t.rotation;

                t.SetParent(
                    pivoVentilador,
                    true
                );

                t.position =
                    posicaoMundo;

                t.rotation =
                    rotacaoMundo;

                Debug.Log(
                    "Ciano: zzzextraventiladorON encontrado " +
                    "e vinculado ao pivô."
                );

                break;
            }
        }

        if (carcaçaVentilador == null)
        {
            Debug.Log(
                "Ciano: zzzextraventiladorON não encontrado."
            );
        }

        for (int i = 0; i < 9; i++)
        {
            string nome =
                "_vfan" + i + "_";

            ventiladores[i] =
                null;

            foreach (Transform t in todos)
            {
                if (t.name == nome)
                {
                    ventiladores[i] =
                        t;

                    renderersVentilador[i] =
                        t.GetComponentsInChildren<Renderer>(
                            true
                        );

                    Vector3 posicaoMundo =
                        t.position;

                    Quaternion rotacaoMundo =
                        t.rotation;

                    t.SetParent(
                        pivoVentilador,
                        true
                    );

                    t.position =
                        posicaoMundo;

                    t.rotation =
                        rotacaoMundo;

                    Debug.Log(
                        "Ciano: " +
                        nome +
                        " encontrado e vinculado ao pivô."
                    );

                    break;
                }
            }

            if (ventiladores[i] == null)
            {
                Debug.Log(
                    "Ciano: " +
                    nome +
                    " não encontrado."
                );
            }
        }

        frameVentilador =
            0;

        tempoVentilador =
            0f;

        for (int i = 0; i < 9; i++)
        {
            if (
                renderersVentilador[i] ==
                null
            )
                continue;

            bool ativo =
                i == 0;

            foreach (
                Renderer renderer
                in renderersVentilador[i]
            )
            {
                if (renderer != null)
                    renderer.enabled =
                        ativo;
            }
        }
    }

    // ============================================================
    // ATUALIZAR VENTILADOR
    // ============================================================

    void AtualizarVentilador()
    {
        if (runtimeVars == null)
            return;

        if (!runtimeVars.engineOn)
            return;

        tempoVentilador +=
            Time.deltaTime *
            velocidadeVentilador;

        int novoFrame =
            Mathf.FloorToInt(
                tempoVentilador
            ) % 9;

        if (
            novoFrame ==
            frameVentilador
        )
            return;

        frameVentilador =
            novoFrame;

        for (int i = 0; i < 9; i++)
        {
            if (
                renderersVentilador[i] ==
                null
            )
                continue;

            bool ativo =
                i == frameVentilador;

            foreach (
                Renderer renderer
                in renderersVentilador[i]
            )
            {
                if (renderer != null)
                    renderer.enabled =
                        ativo;
            }
        }
    }

    // ============================================================
    // ATUALIZAR VELOCIDADE
    // ============================================================

    void AtualizarVelocidadeOnibus()
    {
        if (raizMeshes == null)
            return;

        if (!posicaoInicialRegistrada)
        {
            posicaoAnteriorOnibus =
                raizMeshes.position;

            posicaoInicialRegistrada =
                true;

            velocidadeAtual = 0f;
            velocidadeAnterior = 0f;

            return;
        }

        velocidadeAnterior =
            velocidadeAtual;

        Vector3 deslocamento =
            raizMeshes.position -
            posicaoAnteriorOnibus;

        velocidadeAtual =
            deslocamento.magnitude /
            Mathf.Max(
                Time.deltaTime,
                0.0001f
            );

        posicaoAnteriorOnibus =
            raizMeshes.position;
    }

    // ============================================================
    // TRANSIÇÃO DA VIBRAÇÃO
    // ============================================================

    void AtualizarNivelVibracao()
    {
        if (runtimeVars == null)
            return;

        bool motorLigado =
            runtimeVars.engineOn;

        if (
            motorLigado &&
            !motorAnterior
        )
        {
            tempoPartidaMotor =
                0f;

            tempoDesligamentoMotor =
                999f;

            tempoVibracaoCamera =
                0f;

            cameraVibrando =
                true;
        }

        if (
            !motorLigado &&
            motorAnterior
        )
        {
            tempoDesligamentoMotor =
                0f;

            tempoPartidaMotor =
                999f;

            tempoVibracaoCamera =
                0f;

            cameraVibrando =
                true;
        }

        motorAnterior =
            motorLigado;

        if (!motorLigado)
        {
            tempoDesligamentoMotor +=
                Time.deltaTime;

            float progressoDesligamento =
                Mathf.Clamp01(
                    tempoDesligamentoMotor /
                    duracaoVibracaoDesligamento
                );

            float curva =
                Suavizar(
                    progressoDesligamento
                );

            float intensidadeExtra =
                Mathf.Lerp(
                    intensidadeVibracaoPartida,
                    0f,
                    curva
                );

            nivelVibracao =
                Mathf.MoveTowards(
                    nivelVibracao,
                    0f,
                    Time.deltaTime /
                    duracaoVibracaoDesligamento
                );

            if (
                tempoDesligamentoMotor <
                duracaoVibracaoDesligamento
            )
            {
                float pulso =
                    Mathf.Sin(
                        progressoDesligamento *
                        Mathf.PI
                    );

                nivelVibracao =
                    Mathf.Max(
                        nivelVibracao,
                        pulso *
                        intensidadeExtra
                    );
            }

            return;
        }

        tempoPartidaMotor +=
            Time.deltaTime;

        float fatorPartida =
            0f;

        if (
            tempoPartidaMotor <
            duracaoVibracaoPartida
        )
        {
            float progressoPartida =
                Mathf.Clamp01(
                    tempoPartidaMotor /
                    duracaoVibracaoPartida
                );

            fatorPartida =
                Mathf.Sin(
                    progressoPartida *
                    Mathf.PI
                );

            fatorPartida =
                fatorPartida *
                intensidadeVibracaoPartida;
        }

        float fatorVelocidade;

        if (
            velocidadeAtual <=
            velocidadeParado
        )
        {
            fatorVelocidade =
                1f;
        }
        else if (
            velocidadeAtual >=
            velocidadeSemVibracao
        )
        {
            fatorVelocidade =
                0f;
        }
        else
        {
            float progressoVelocidade =
                Mathf.InverseLerp(
                    velocidadeParado,
                    velocidadeSemVibracao,
                    velocidadeAtual
                );

            fatorVelocidade =
                1f -
                Suavizar(
                    progressoVelocidade
                );
        }

        float alvoNormal =
            fatorVelocidade *
            intensidadeVibracaoNormal;

        float duracaoTransicao;

        if (
            alvoNormal <
            nivelVibracao
        )
        {
            duracaoTransicao =
                duracaoTransicaoAceleracao;
        }
        else
        {
            duracaoTransicao =
                duracaoTransicaoParado;
        }

        if (duracaoTransicao <= 0.001f)
        {
            nivelVibracao =
                alvoNormal;
        }
        else
        {
            nivelVibracao =
                Mathf.MoveTowards(
                    nivelVibracao,
                    alvoNormal,
                    Time.deltaTime /
                    duracaoTransicao
                );
        }

        if (fatorPartida > 0f)
        {
            nivelVibracao =
                Mathf.Max(
                    nivelVibracao,
                    fatorPartida
                );
        }
    }

    // ============================================================
    // OBTER VIBRAÇÃO PADRÃO
    // ============================================================

    Quaternion ObterVibracaoPadrao()
    {
        float rotacaoX =
            Mathf.Sin(
                tempoVibracao *
                velocidadeVibracaoCambio
            ) *
            intensidadeVibracaoCambio *
            nivelVibracao;

        float rotacaoY =
            Mathf.Sin(
                tempoVibracao *
                velocidadeVibracaoCambio *
                1.85f
            ) *
            intensidadeVibracaoCambio *
            0.60f *
            nivelVibracao;

        float rotacaoZ =
            Mathf.Sin(
                tempoVibracao *
                velocidadeVibracaoCambio *
                2.35f
            ) *
            intensidadeVibracaoCambio *
            0.40f *
            nivelVibracao;

        return Quaternion.Euler(
            rotacaoX,
            rotacaoY,
            rotacaoZ
        );
    }

    // ============================================================
    // VIBRAÇÃO
    // ============================================================

    void AtualizarVibracao()
    {
        if (runtimeVars == null)
            return;

        AtualizarVelocidadeOnibus();

        AtualizarNivelVibracao();

        if (
            runtimeVars.engineOn ||
            nivelVibracao > 0.001f
        )
        {
            tempoVibracao +=
                Time.deltaTime;
        }
        else
        {
            tempoVibracao = 0f;
        }

        Quaternion vibracao =
            ObterVibracaoPadrao();

        if (pivoCambio != null)
        {
            pivoCambio.localRotation =
                rotacaoBaseCambio *
                vibracao;
        }

        if (pivoVentilador != null)
        {
            pivoVentilador.localRotation =
                vibracao;
        }

        if (
            sunshade != null &&
            sunshade.pivo != null
        )
        {
            Quaternion rotacaoBaseSunshade =
                Quaternion.AngleAxis(
                    sunshade.anguloAtual,
                    sunshade.eixo
                );

            sunshade.pivo.localRotation =
                rotacaoBaseSunshade *
                vibracao;
        }
    }

    // ============================================================
    // PREPARAR PORTA
    // ============================================================

    void PrepararPorta(int id)
    {
        Transform portaTransform =
            CianoUtils.GetRightDoorTransform(id);

        if (portaTransform == null)
        {
            Debug.Log(
                "Ciano: rightdoor" +
                id +
                " não encontrada."
            );

            return;
        }

        MEPCadaPortaBus door =
            portaTransform.GetComponent<MEPCadaPortaBus>();

        if (door == null)
        {
            Debug.Log(
                "Ciano: MEPCadaPortaBus não encontrado na rightdoor" +
                id
            );

            return;
        }

        Porta porta =
            new Porta(id);

        porta.door =
            door;

        if (id == 1)
        {
            porta.inicioFolha2Abrir =
                inicioFolha2AbrirPorta1;

            porta.inicioFolha1Fechar =
                inicioFolha1FecharPorta1;
        }
        else if (id == 2)
        {
            porta.inicioFolha2Abrir =
                inicioFolha2AbrirPorta2;

            porta.inicioFolha1Fechar =
                inicioFolha1FecharPorta2;
        }
        else if (id == 3)
        {
            porta.inicioFolha2Abrir =
                inicioFolha2AbrirPorta3;

            porta.inicioFolha1Fechar =
                inicioFolha1FecharPorta3;
        }

        if (
            door.objPortaFolha1 != null &&
            door.objPortaFolha1.Length > 0
        )
        {
            porta.folha1 =
                door.objPortaFolha1[0];
        }

        if (
            door.objPortaFolha2 != null &&
            door.objPortaFolha2.Length > 0
        )
        {
            porta.folha2 =
                door.objPortaFolha2[0];
        }

        if (porta.folha1 != null)
        {
            porta.rotacaoFechada1 =
                porta.folha1.localRotation;

            porta.rotacaoAberta1 =
                porta.rotacaoFechada1 *
                Quaternion.Euler(
                    0f,
                    -90f,
                    0f
                );
        }

        if (porta.folha2 != null)
        {
            porta.rotacaoFechada2 =
                porta.folha2.localRotation;

            porta.rotacaoAberta2 =
                porta.rotacaoFechada2 *
                Quaternion.Euler(
                    0f,
                    90f,
                    0f
                );
        }

        door.tipoAnim =
            MEPCadaPortaBus.tipoAnimacaoPorta.customScript;

        porta.estadoAnterior =
            door.isOpen;

        porta.tempo =
            0f;

        porta.progressoFolha1 =
            door.isOpen ? 1f : 0f;

        porta.progressoFolha2 =
            door.isOpen ? 1f : 0f;

        door.allowPaxWalk =
            false;

        portas.Add(porta);

        Debug.Log(
            "Ciano: rightdoor" +
            id +
            " preparada."
        );
    }

    // ============================================================
    // UPDATE
    // ============================================================

    void Update()
    {
        AtualizarCliques();

        AtualizarRetrovisores();

        AtualizarAnimacaoPivo(capo);
        AtualizarAnimacaoPivo(capolet);
        AtualizarAnimacaoPivo(sunshade);

        AtualizarAnimacaoPivo(tampa1);
        AtualizarAnimacaoPivo(tampa2);

        AtualizarJanelasMotorista();

        AtualizarFreio();

        AtualizarCambio();

        AtualizarVentilador();

        foreach (Porta porta in portas)
        {
            AtualizarPorta(porta);
        }

        // ========================================================
        // CATRAÇA
        // ========================================================

        AtualizarCatraca();

        // ========================================================
        // MOBITEC
        // ========================================================

        AtualizarEstadoEletrica();

        ProcessarClique();
    }

    // ============================================================
    // LATE UPDATE
    // ============================================================

    void LateUpdate()
    {
        AtualizarVibracao();

        AtualizarVibracaoCamera();

        // ========================================================
        // MOBITEC
        // ========================================================

        if (
            !string.IsNullOrEmpty(
                pastaDestinoAtual
            )
        )
        {
            AtualizarTexturasDosLetreiros();
        }
    }

    // ============================================================
    // FREIO
    // ============================================================

    void AtualizarFreio()
    {
        if (
            parkingBrake == null ||
            parkingBrake.pivo == null ||
            runtimeVars == null
        )
            return;

        bool ativo =
            runtimeVars.parkingBrakeActive;

        parkingBrake.estado =
            ativo;

        float alvo =
            ativo
                ? 0f
                : 75.9f;

        parkingBrake.anguloAtual =
            Mathf.MoveTowards(
                parkingBrake.anguloAtual,
                alvo,
                parkingBrake.velocidade *
                Time.deltaTime
            );

        parkingBrake.pivo.localRotation =
            Quaternion.AngleAxis(
                parkingBrake.anguloAtual,
                parkingBrake.eixo
            );
    }

    // ============================================================
    // ATUALIZAR PORTA
    // ============================================================

    void AtualizarPorta(Porta porta)
    {
        if (porta.door == null)
            return;

        bool aberta =
            porta.door.isOpen;

        if (aberta != porta.estadoAnterior)
        {
            porta.estadoAnterior =
                aberta;

            porta.batidaFolha1Ativa =
                false;

            porta.batidaFolha1Executada =
                false;

            porta.tempoBatidaFolha1 =
                0f;

            porta.batidaFolha2Ativa =
                false;

            porta.batidaFolha2Executada =
                false;

            porta.tempoBatidaFolha2 =
                0f;
        }

        if (aberta)
            AnimarAbertura(porta);
        else
            AnimarFechamento(porta);
    }

    // ============================================================
    // ABERTURA
    // ============================================================

    void AnimarAbertura(Porta porta)
    {
        porta.progressoFolha1 =
            Mathf.MoveTowards(
                porta.progressoFolha1,
                1f,
                Time.deltaTime /
                tempoFolha1Abrir
            );

        float progresso1Suave =
            Suavizar(
                porta.progressoFolha1
            );

        if (porta.folha1 != null)
        {
            Quaternion rotacaoBase =
                Quaternion.Slerp(
                    porta.rotacaoFechada1,
                    porta.rotacaoAberta1,
                    progresso1Suave
                );

            if (
                porta.progressoFolha1 >=
                (1f - inicioBatidaPorta)
            )
            {
                if (
                    !porta.batidaFolha1Executada
                )
                {
                    porta.batidaFolha1Executada =
                        true;

                    porta.batidaFolha1Ativa =
                        true;

                    porta.tempoBatidaFolha1 =
                        0f;
                }
            }

            Quaternion batida =
                ObterRotacaoBatida(
                    porta,
                    1
                );

            porta.folha1.localRotation =
                rotacaoBase *
                batida;
        }

        if (
            porta.progressoFolha1 >=
            porta.inicioFolha2Abrir
        )
        {
            porta.progressoFolha2 =
                Mathf.MoveTowards(
                    porta.progressoFolha2,
                    1f,
                    Time.deltaTime /
                    tempoFolha2Abrir
                );
        }

        float progresso2Suave =
            Suavizar(
                porta.progressoFolha2
            );

        if (porta.folha2 != null)
        {
            Quaternion rotacaoBase =
                Quaternion.Slerp(
                    porta.rotacaoFechada2,
                    porta.rotacaoAberta2,
                    progresso2Suave
                );

            if (
                porta.progressoFolha2 >=
                (1f - inicioBatidaPorta)
            )
            {
                if (
                    !porta.batidaFolha2Executada
                )
                {
                    porta.batidaFolha2Executada =
                        true;

                    porta.batidaFolha2Ativa =
                        true;

                    porta.tempoBatidaFolha2 =
                        0f;
                }
            }

            Quaternion batida =
                ObterRotacaoBatida(
                    porta,
                    2
                );

            porta.folha2.localRotation =
                rotacaoBase *
                batida;
        }

        bool folha1Aberta =
            porta.progressoFolha1 >=
            0.999f;

        bool folha2Aberta =
            porta.progressoFolha2 >=
            0.999f;

        porta.door.allowPaxWalk =
            folha1Aberta &&
            folha2Aberta;
    }

    // ============================================================
    // FECHAMENTO
    // ============================================================

    void AnimarFechamento(Porta porta)
    {
        porta.progressoFolha2 =
            Mathf.MoveTowards(
                porta.progressoFolha2,
                0f,
                Time.deltaTime /
                tempoFolha2Fechar
            );

        float progresso2Suave =
            Suavizar(
                porta.progressoFolha2
            );

        if (porta.folha2 != null)
        {
            Quaternion rotacaoBase =
                Quaternion.Slerp(
                    porta.rotacaoFechada2,
                    porta.rotacaoAberta2,
                    progresso2Suave
                );

            if (
                porta.progressoFolha2 <=
                inicioBatidaPorta
            )
            {
                if (
                    !porta.batidaFolha2Executada
                )
                {
                    porta.batidaFolha2Executada =
                        true;

                    porta.batidaFolha2Ativa =
                        true;

                    porta.tempoBatidaFolha2 =
                        0f;
                }
            }

            Quaternion batida =
                ObterRotacaoBatida(
                    porta,
                    2
                );

            porta.folha2.localRotation =
                rotacaoBase *
                batida;
        }

        float limiteInicioFolha1 =
            1f -
            porta.inicioFolha1Fechar;

        if (
            porta.progressoFolha2 <=
            limiteInicioFolha1
        )
        {
            porta.progressoFolha1 =
                Mathf.MoveTowards(
                    porta.progressoFolha1,
                    0f,
                    Time.deltaTime /
                    tempoFolha1Fechar
                );
        }

        float progresso1Suave =
            Suavizar(
                porta.progressoFolha1
            );

        if (porta.folha1 != null)
        {
            Quaternion rotacaoBase =
                Quaternion.Slerp(
                    porta.rotacaoFechada1,
                    porta.rotacaoAberta1,
                    progresso1Suave
                );

            if (
                porta.progressoFolha1 <=
                inicioBatidaPorta
            )
            {
                if (
                    !porta.batidaFolha1Executada
                )
                {
                    porta.batidaFolha1Executada =
                        true;

                    porta.batidaFolha1Ativa =
                        true;

                    porta.tempoBatidaFolha1 =
                        0f;
                }
            }

            Quaternion batida =
                ObterRotacaoBatida(
                    porta,
                    1
                );

            porta.folha1.localRotation =
                rotacaoBase *
                batida;
        }

        porta.door.allowPaxWalk =
            false;
    }

    // ============================================================
    // BATIDA DA PORTA
    // ============================================================

    Quaternion ObterRotacaoBatida(
        Porta porta,
        int folha
    )
    {
        bool ativa;
        float tempo;

        if (folha == 1)
        {
            ativa =
                porta.batidaFolha1Ativa;

            tempo =
                porta.tempoBatidaFolha1;
        }
        else
        {
            ativa =
                porta.batidaFolha2Ativa;

            tempo =
                porta.tempoBatidaFolha2;
        }

        if (!ativa)
            return Quaternion.identity;

        tempo +=
            Time.deltaTime;

        if (folha == 1)
        {
            porta.tempoBatidaFolha1 =
                tempo;
        }
        else
        {
            porta.tempoBatidaFolha2 =
                tempo;
        }

        if (
            tempo >=
            duracaoBatidaPorta
        )
        {
            if (folha == 1)
            {
                porta.batidaFolha1Ativa =
                    false;
            }
            else
            {
                porta.batidaFolha2Ativa =
                    false;
            }

            return Quaternion.identity;
        }

        float amplitude;

        if (intensidadeBatidaPorta <= 1)
        {
            amplitude =
                0.6f;
        }
        else if (intensidadeBatidaPorta == 2)
        {
            amplitude =
                1.2f;
        }
        else
        {
            amplitude =
                2.0f;
        }

        float progresso =
            Mathf.Clamp01(
                tempo /
                duracaoBatidaPorta
            );

        float amortecimento =
            1f -
            progresso;

        float oscilacao =
            Mathf.Sin(
                tempo *
                frequenciaBatidaPorta
            ) *
            amortecimento *
            amplitude;

        if (folha == 1)
        {
            oscilacao *= -1f;
        }

        return Quaternion.Euler(
            0f,
            oscilacao,
            0f
        );
    }

    // ============================================================
    // SUAVIZAÇÃO
    // ============================================================

    float Suavizar(float valor)
    {
        return valor * valor *
               (3f - 2f * valor);
    }

    // ============================================================
    // VIBRAÇÃO DA CÂMERA DO MOTORISTA
    // ============================================================

    float intensidadeVibracaoCamera = 0.08f;
    float frequenciaVibracaoCamera = 58f;
    float delayVibracaoCamera = 0.12f;
    float tempoVibracaoCamera = 999f;
    bool cameraVibrando = false;

    Quaternion ultimaVibracaoCamera =
        Quaternion.identity;

    void AtualizarVibracaoCamera()
    {
        Camera camera =
            Camera.main;

        if (camera == null)
            return;

        if (
            ultimaVibracaoCamera !=
            Quaternion.identity
        )
        {
            camera.transform.localRotation =
                camera.transform.localRotation *
                Quaternion.Inverse(
                    ultimaVibracaoCamera
                );

            ultimaVibracaoCamera =
                Quaternion.identity;
        }

        if (
            CianoUtils.GetCurrentCameraMode() !=
            CianoUtils.CameraMode.internalCam
        )
        {
            cameraVibrando =
                false;

            return;
        }

        if (!cameraVibrando)
            return;

        bool motorLigado =
            runtimeVars != null &&
            runtimeVars.engineOn;

        float duracao;

        if (motorLigado)
        {
            duracao =
                duracaoVibracaoPartida;
        }
        else
        {
            duracao =
                duracaoVibracaoDesligamento;
        }

        tempoVibracaoCamera +=
            Time.deltaTime;

        if (
            tempoVibracaoCamera <
            delayVibracaoCamera
        )
        {
            return;
        }

        float tempoAnimacao =
            tempoVibracaoCamera -
            delayVibracaoCamera;

        float progresso =
            Mathf.Clamp01(
                tempoAnimacao /
                duracao
            );

        if (progresso >= 1f)
        {
            cameraVibrando =
                false;

            return;
        }

        float envelope =
            Mathf.Sin(
                progresso *
                Mathf.PI
            );

        float tempo =
            tempoAnimacao *
            frequenciaVibracaoCamera;

        float intensidade =
            intensidadeVibracaoCamera *
            envelope;

        float x =
            Mathf.Sin(
                tempo
            ) *
            intensidade;

        float y =
            Mathf.Sin(
                tempo *
                1.85f
            ) *
            intensidade *
            0.60f;

        float z =
            Mathf.Sin(
                tempo *
                2.35f
            ) *
            intensidade *
            0.40f;

        Quaternion vibracaoCamera =
            Quaternion.Euler(
                x,
                y,
                z
            );

        camera.transform.localRotation =
            camera.transform.localRotation *
            vibracaoCamera;

        ultimaVibracaoCamera =
            vibracaoCamera;
    }

    // ============================================================
    //
    // CATRAÇA - PREPARAÇÃO
    //
    // ============================================================

    void PrepararCatraca()
    {
        if (raizMeshes == null)
            return;

        Transform[] todos =
            raizMeshes.GetComponentsInChildren<Transform>(
                true
            );

        objetosCatraca.Clear();

        foreach (Transform t in todos)
        {
            if (t == null)
                continue;

            if (
                t.name.IndexOf(
                    "_catraca_",
                    System.StringComparison.OrdinalIgnoreCase
                ) >= 0
            )
            {
                objetosCatraca.Add(t);

                Debug.Log(
                    "Ciano Catraca: objeto encontrado: " +
                    t.name
                );
            }
        }

        if (objetosCatraca.Count == 0)
        {
            Debug.Log(
                "Ciano Catraca: nenhum objeto _catraca_ encontrado."
            );

            return;
        }

        GameObject objetoPivo =
            new GameObject(
                "CianoPivo_Catraca"
            );

        pivoCatraca =
            objetoPivo.transform;

        pivoCatraca.SetParent(
            raizMeshes,
            false
        );

        pivoCatraca.localPosition =
            posicaoPivoCatraca;

        pivoCatraca.localRotation =
            Quaternion.identity;

        foreach (
            Transform objeto
            in objetosCatraca
        )
        {
            Vector3 posicaoMundo =
                objeto.position;

            Quaternion rotacaoMundo =
                objeto.rotation;

            objeto.SetParent(
                pivoCatraca,
                true
            );

            objeto.position =
                posicaoMundo;

            objeto.rotation =
                rotacaoMundo;
        }

        anguloCatraca = 0f;
        anguloAlvoCatraca = 0f;
        catracaAnimando = false;

        pivoCatraca.localRotation =
            Quaternion.identity;

        Debug.Log(
            "Ciano Catraca: preparada. " +
            "Objetos encontrados: " +
            objetosCatraca.Count
        );
    }

    // ============================================================
    //
    // ATUALIZAR CATRAÇA
    //
    // ============================================================

    void AtualizarCatraca()
    {
        if (
            pivoCatraca == null ||
            objetosCatraca.Count == 0
        )
        {
            return;
        }

        if (
            CianoUtils.IsTurnstile1Activated()
        )
        {
            CianoUtils.ResetTurnstile1Activated();

            anguloAlvoCatraca +=
                incrementoCatraca;

            catracaAnimando =
                true;

            Debug.Log(
                "Ciano Catraca: passageiro passou. " +
                "Novo ângulo alvo = " +
                anguloAlvoCatraca +
                " graus."
            );
        }

        if (!catracaAnimando)
            return;

        anguloCatraca =
            Mathf.MoveTowards(
                anguloCatraca,
                anguloAlvoCatraca,
                velocidadeCatraca *
                Time.deltaTime
            );

        pivoCatraca.localRotation =
            Quaternion.AngleAxis(
                anguloCatraca,
                Vector3.up
            );

        if (
            Mathf.Abs(
                anguloCatraca -
                anguloAlvoCatraca
            ) <
            0.01f
        )
        {
            anguloCatraca =
                anguloAlvoCatraca;

            pivoCatraca.localRotation =
                Quaternion.AngleAxis(
                    anguloCatraca,
                    Vector3.up
                );

            catracaAnimando =
                false;
        }
    }

    // ============================================================
    //
    // MOBITEC - DISPLAY 3D DE NÚMEROS + DISPLAY NATIVO
    //
    // ============================================================

    GameObject[] botoesNumericos = new GameObject[10];

    GameObject botaoEnter;
    GameObject botaoCancel;

    GameObject mobitecDisplay;
    GameObject mobitecFundo;
    GameObject mobitecPrincipal;

    string codigoDigitado = "";

    const int ClickableLayerMask = 1 << 13;

    bool estadoEletricaAnterior = false;

    Renderer[][] renderersBotoes =
        new Renderer[12][];

    Material[][] materiaisOriginaisBotoes =
        new Material[12][];

    Renderer[] renderersFundo;

    Material[] materiaisOriginaisFundo;

    Renderer[] renderersMobitecPrincipal;

    Material[] materiaisOriginaisMobitecPrincipal;

    Material materialIBISN;
    Material materialIBISA;

    GameObject botaoAtualmenteAceso;

    GameObject[,] numerosDisplay =
        new GameObject[6, 10];

    const int QuantidadePosicoes = 6;

    List<string> destinosEncontrados =
        new List<string>();

    string ultimoCodigoPesquisado = "";

    int indiceDestinoAtual = -1;

    string pastaDestinoAtual = "";

    Dictionary<string, Texture2D> texturasDestinoCarregadas =
        new Dictionary<string, Texture2D>();

    string[] tagsLetreiros =
    {
        "_display_front_",
        "_display_back_",
        "_old_display_route_",
        "_old_display_destination_",
        "_plate_route_small_",
        "_plate_side_",
        "_plate_front1_",
        "_plate_front2_",
        "_scroll_panel1_",
        "_scroll_panel2_",
        "_scroll_panel3_"
    };

    // ============================================================
    // INICIALIZAÇÃO DO MOBITEC
    // ============================================================

    void IniciarMobitec()
    {
        if (raizMeshes == null)
        {
            Debug.Log(
                "Ciano Mobitec: raiz das meshes não encontrada."
            );

            return;
        }

        if (runtimeVars == null)
        {
            Debug.Log(
                "Ciano Mobitec: RuntimeModVariables não encontrada."
            );
        }

        foreach (
            Transform t
            in raizMeshes.GetComponentsInChildren<Transform>(true)
        )
        {
            string nome = t.name;

            for (int i = 0; i <= 9; i++)
            {
                if (nome.Contains("_mobitec_" + i + "_"))
                {
                    botoesNumericos[i] =
                        t.gameObject;
                }
            }

            if (nome.Contains("_mobitec_enter_"))
            {
                botaoEnter =
                    t.gameObject;
            }

            if (nome.Contains("_mobitec_cancel_"))
            {
                botaoCancel =
                    t.gameObject;
            }

            if (nome.Contains("_mobitec_display_"))
            {
                mobitecDisplay =
                    t.gameObject;
            }

            if (nome.Contains("_mobitec_fundo_"))
            {
                mobitecFundo =
                    t.gameObject;
            }

            if (nome == "_mobitec_")
            {
                mobitecPrincipal =
                    t.gameObject;
            }

            for (int numero = 0; numero <= 9; numero++)
            {
                for (
                    int posicao = 1;
                    posicao <= QuantidadePosicoes;
                    posicao++
                )
                {
                    string prefixo =
                        "_mobitec_num" +
                        numero +
                        "_pos" +
                        posicao +
                        "_";

                    if (nome.Contains(prefixo))
                    {
                        numerosDisplay[
                            posicao - 1,
                            numero
                        ] = t.gameObject;

                        Debug.Log(
                            "Ciano Mobitec: número " +
                            numero +
                            " da posição " +
                            posicao +
                            " encontrado."
                        );
                    }
                }
            }
        }

        for (int i = 0; i <= 9; i++)
        {
            GuardarMateriaisBotao(
                i,
                botoesNumericos[i]
            );
        }

        GuardarMateriaisBotao(
            10,
            botaoEnter
        );

        GuardarMateriaisBotao(
            11,
            botaoCancel
        );

        if (mobitecFundo != null)
        {
            renderersFundo =
                mobitecFundo.GetComponentsInChildren<Renderer>(true);

            materiaisOriginaisFundo =
                GuardarMateriais(
                    renderersFundo
                );
        }

        if (mobitecPrincipal != null)
        {
            renderersMobitecPrincipal =
                mobitecPrincipal.GetComponentsInChildren<Renderer>(true);

            materiaisOriginaisMobitecPrincipal =
                GuardarMateriais(
                    renderersMobitecPrincipal
                );

            Debug.Log(
                "Ciano Mobitec: objeto _mobitec_ encontrado."
            );
        }
        else
        {
            Debug.Log(
                "Ciano Mobitec: objeto _mobitec_ NÃO encontrado."
            );
        }

        CriarMateriaisAcesos();

        for (int i = 0; i <= 9; i++)
        {
            if (botoesNumericos[i] != null)
            {
                CianoUtils.CreateSphereColliderForButton(
                    botoesNumericos[i]
                );

                Debug.Log(
                    "Ciano Mobitec: botão " +
                    i +
                    " encontrado."
                );
            }
            else
            {
                Debug.Log(
                    "Ciano Mobitec: botão " +
                    i +
                    " NÃO encontrado."
                );
            }
        }

        if (botaoEnter != null)
        {
            CianoUtils.CreateSphereColliderForButton(
                botaoEnter
            );

            Debug.Log(
                "Ciano Mobitec: botão ENTER encontrado."
            );
        }
        else
        {
            Debug.Log(
                "Ciano Mobitec: botão ENTER NÃO encontrado."
            );
        }

        if (botaoCancel != null)
        {
            CianoUtils.CreateSphereColliderForButton(
                botaoCancel
            );

            Debug.Log(
                "Ciano Mobitec: botão CANCEL encontrado."
            );
        }
        else
        {
            Debug.Log(
                "Ciano Mobitec: botão CANCEL NÃO encontrado."
            );
        }

        LimparDisplayNumerico();

        if (mobitecDisplay != null)
        {
            mobitecDisplay.SetActive(false);
        }

        DesligarIluminacaoMobitec();

        if (runtimeVars != null)
        {
            estadoEletricaAnterior =
                runtimeVars.electricOn;

            AtualizarIluminacaoEletrica();
        }

        Debug.Log(
            "Ciano Mobitec: sistema iniciado."
        );
    }

    // ============================================================
    // GUARDA MATERIAIS DE UM BOTÃO
    // ============================================================

    void GuardarMateriaisBotao(
        int indice,
        GameObject botao
    )
    {
        if (botao == null)
            return;

        renderersBotoes[indice] =
            botao.GetComponentsInChildren<Renderer>(true);

        materiaisOriginaisBotoes[indice] =
            GuardarMateriais(
                renderersBotoes[indice]
            );
    }

    // ============================================================
    // GUARDA MATERIAIS
    // ============================================================

    Material[] GuardarMateriais(
        Renderer[] renderers
    )
    {
        if (renderers == null)
            return null;

        Material[] materiais =
            new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null)
            {
                materiais[i] =
                    renderers[i].material;
            }
        }

        return materiais;
    }

    // ============================================================
    // ATUALIZA ESTADO DA ELÉTRICA
    // ============================================================

    void AtualizarEstadoEletrica()
    {
        if (runtimeVars == null)
            return;

        bool eletricaLigada =
            runtimeVars.electricOn;

        if (eletricaLigada != estadoEletricaAnterior)
        {
            estadoEletricaAnterior =
                eletricaLigada;

            AtualizarIluminacaoEletrica();

            if (!eletricaLigada)
            {
                codigoDigitado = "";

                LimparDisplayNumerico();

                if (mobitecDisplay != null)
                {
                    mobitecDisplay.SetActive(false);
                }

                botaoAtualmenteAceso =
                    null;

                destinosEncontrados.Clear();

                ultimoCodigoPesquisado = "";

                indiceDestinoAtual = -1;

                pastaDestinoAtual = "";

                texturasDestinoCarregadas.Clear();

                Debug.Log(
                    "Ciano Mobitec: elétrica desligada. Sistema resetado."
                );
            }
        }
    }

    // ============================================================
    // ILUMINAÇÃO DA ELÉTRICA
    // ============================================================

    void AtualizarIluminacaoEletrica()
    {
        if (runtimeVars == null)
            return;

        if (runtimeVars.electricOn)
        {
            LigarIluminacaoMobitec();

            Debug.Log(
                "Ciano Mobitec: elétrica ligada. IBISN aplicado."
            );
        }
        else
        {
            DesligarIluminacaoMobitec();

            Debug.Log(
                "Ciano Mobitec: elétrica desligada."
            );
        }
    }

    // ============================================================
    // CRIA OS MATERIAIS IBISN E IBISA
    // ============================================================

    void CriarMateriaisAcesos()
    {
        string diretorio =
            CianoUtils.GetCurrentPlayerModBaseDir();

        string diretorioTexturas =
            ModFileUtilsForPlayer.CombinePaths(
                diretorio,
                "textures"
            );

        string caminhoIBISN =
            ModFileUtilsForPlayer.CombinePaths(
                diretorioTexturas,
                "IBISN.png"
            );

        string caminhoIBISA =
            ModFileUtilsForPlayer.CombinePaths(
                diretorioTexturas,
                "IBISA.png"
            );

        Texture2D texturaIBISN =
            CianoUtils.LoadTexture(
                caminhoIBISN
            );

        Texture2D texturaIBISA =
            CianoUtils.LoadTexture(
                caminhoIBISA
            );

        if (texturaIBISN == null)
        {
            Debug.Log(
                "Ciano Mobitec: IBISN.png não encontrada."
            );
        }

        if (texturaIBISA == null)
        {
            Debug.Log(
                "Ciano Mobitec: IBISA.png não encontrada."
            );
        }

        if (texturaIBISN != null)
        {
            materialIBISN =
                CianoUtils.CreateMaterial(
                    new CianoUtils.MaterialSettings
                    {
                        type =
                            CianoUtils.MaterialType.Emissive,

                        mainTexture =
                            texturaIBISN,

                        emissionTexture =
                            texturaIBISN
                    }
                );
        }

        if (texturaIBISA != null)
        {
            materialIBISA =
                CianoUtils.CreateMaterial(
                    new CianoUtils.MaterialSettings
                    {
                        type =
                            CianoUtils.MaterialType.Emissive,

                        mainTexture =
                            texturaIBISA,

                        emissionTexture =
                            texturaIBISA
                    }
                );
        }
    }

    // ============================================================
    // APLICA MATERIAL
    // ============================================================

    void AplicarMaterial(
        Renderer[] renderers,
        Material material
    )
    {
        if (renderers == null)
            return;

        if (material == null)
            return;

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null)
            {
                renderers[i].material =
                    material;
            }
        }
    }

    // ============================================================
    // LIGA O MOBITEC
    // ============================================================

    void LigarIluminacaoMobitec()
    {
        AplicarMaterial(
            renderersMobitecPrincipal,
            materialIBISN
        );

        AplicarMaterial(
            renderersFundo,
            materialIBISN
        );

        for (int i = 0; i <= 11; i++)
        {
            AplicarMaterial(
                renderersBotoes[i],
                materialIBISN
            );
        }
    }

    // ============================================================
    // DESLIGA O MOBITEC
    // ============================================================

    void DesligarIluminacaoMobitec()
    {
        RestaurarMateriais(
            renderersMobitecPrincipal,
            materiaisOriginaisMobitecPrincipal
        );

        RestaurarMateriais(
            renderersFundo,
            materiaisOriginaisFundo
        );

        for (int i = 0; i <= 11; i++)
        {
            RestaurarMateriais(
                renderersBotoes[i],
                materiaisOriginaisBotoes[i]
            );
        }

        botaoAtualmenteAceso =
            null;
    }

    // ============================================================
    // RESTAURA MATERIAIS
    // ============================================================

    void RestaurarMateriais(
        Renderer[] renderers,
        Material[] materiais
    )
    {
        if (renderers == null)
            return;

        if (materiais == null)
            return;

        int quantidade =
            Mathf.Min(
                renderers.Length,
                materiais.Length
            );

        for (int i = 0; i < quantidade; i++)
        {
            if (
                renderers[i] != null
                &&
                materiais[i] != null
            )
            {
                renderers[i].material =
                    materiais[i];
            }
        }
    }

    // ============================================================
    // ACENDE UM BOTÃO COM IBISA
    // ============================================================

    void AcenderBotao(
        GameObject botao
    )
    {
        if (botao == null)
            return;

        if (
            botaoAtualmenteAceso != null
            &&
            botaoAtualmenteAceso != botao
        )
        {
            AplicarMaterialAoBotao(
                botaoAtualmenteAceso,
                materialIBISN
            );
        }

        AplicarMaterialAoBotao(
            botao,
            materialIBISA
        );

        botaoAtualmenteAceso =
            botao;
    }

    // ============================================================
    // APLICA MATERIAL AO BOTÃO
    // ============================================================

    void AplicarMaterialAoBotao(
        GameObject botao,
        Material material
    )
    {
        if (botao == null)
            return;

        if (material == null)
            return;

        Renderer[] renderers =
            botao.GetComponentsInChildren<Renderer>(true);

        AplicarMaterial(
            renderers,
            material
        );
    }

    // ============================================================
    // CLIQUES
    // ============================================================

    void ProcessarClique()
    {
        if (
            runtimeVars == null
            ||
            !runtimeVars.electricOn
        )
        {
            return;
        }

        if (!CianoInput.GameInputAllowed())
            return;

        bool cliquesPermitidos =
            CianoInput.IsHoldingCtrl()
            ||
            CianoUtils.GetCurrentCameraMode()
            ==
            CianoUtils.CameraMode.buttonsCam;

        if (!cliquesPermitidos)
            return;

        if (!CianoInput.GetMouseButtonDown(0))
            return;

        Camera cam =
            Camera.main;

        if (cam == null)
            return;

        Ray ray =
            cam.ScreenPointToRay(
                Input.mousePosition
            );

        RaycastHit hit;

        if (!Physics.Raycast(
                ray,
                out hit,
                100f,
                ClickableLayerMask,
                QueryTriggerInteraction.Collide))
        {
            return;
        }

        GameObject clicado =
            hit.transform.gameObject;

        for (int i = 0; i <= 9; i++)
        {
            if (botoesNumericos[i] == null)
                continue;

            if (
                botoesNumericos[i] == clicado
                ||
                clicado.transform.IsChildOf(
                    botoesNumericos[i].transform
                )
            )
            {
                AcenderBotao(
                    botoesNumericos[i]
                );

                AdicionarNumero(i);

                return;
            }
        }

        if (botaoEnter != null)
        {
            if (
                botaoEnter == clicado
                ||
                clicado.transform.IsChildOf(
                    botaoEnter.transform
                )
            )
            {
                AcenderBotao(
                    botaoEnter
                );

                ConfirmarCodigo();

                return;
            }
        }

        if (botaoCancel != null)
        {
            if (
                botaoCancel == clicado
                ||
                clicado.transform.IsChildOf(
                    botaoCancel.transform
                )
            )
            {
                AcenderBotao(
                    botaoCancel
                );

                CancelarCodigo();

                return;
            }
        }
    }

    // ============================================================
    // ADICIONAR NÚMERO
    // ============================================================

    void AdicionarNumero(
        int numero
    )
    {
        if (
            codigoDigitado.Length
            >= QuantidadePosicoes
        )
        {
            Debug.Log(
                "Ciano Mobitec: limite de " +
                QuantidadePosicoes +
                " dígitos atingido."
            );

            return;
        }

        int indicePosicao =
            codigoDigitado.Length;

        codigoDigitado +=
            numero.ToString();

        MostrarNumero(
            indicePosicao,
            numero
        );

        Debug.Log(
            "Ciano Mobitec: botão " +
            numero +
            " pressionado."
        );

        Debug.Log(
            "Ciano Mobitec: código atual = " +
            codigoDigitado
        );
    }

    // ============================================================
    // MOSTRAR NÚMERO
    // ============================================================

    void MostrarNumero(
        int indicePosicao,
        int numero
    )
    {
        if (
            indicePosicao < 0
            ||
            indicePosicao >= QuantidadePosicoes
        )
        {
            return;
        }

        for (int i = 0; i <= 9; i++)
        {
            GameObject objeto =
                numerosDisplay[
                    indicePosicao,
                    i
                ];

            if (objeto != null)
            {
                objeto.SetActive(false);
            }
        }

        GameObject numeroSelecionado =
            numerosDisplay[
                indicePosicao,
                numero
            ];

        if (numeroSelecionado != null)
        {
            numeroSelecionado.SetActive(true);
        }
        else
        {
            Debug.Log(
                "Ciano Mobitec: número " +
                numero +
                " da posição " +
                (indicePosicao + 1) +
                " NÃO encontrado."
            );
        }
    }

    // ============================================================
    // LIMPAR DISPLAY NUMÉRICO
    // ============================================================

    void LimparDisplayNumerico()
    {
        for (
            int posicao = 0;
            posicao < QuantidadePosicoes;
            posicao++
        )
        {
            for (
                int numero = 0;
                numero <= 9;
                numero++
            )
            {
                GameObject objeto =
                    numerosDisplay[
                        posicao,
                        numero
                    ];

                if (objeto != null)
                {
                    objeto.SetActive(false);
                }
            }
        }
    }

    // ============================================================
    // NORMALIZA O CÓDIGO PARA PESQUISA
    // ============================================================

    string NormalizarCodigo(
        string codigo
    )
    {
        if (string.IsNullOrEmpty(codigo))
            return "";

        string resultado =
            codigo.Trim()
            .ToLower();

        resultado =
            resultado.Replace(
                "-",
                ""
            );

        resultado =
            resultado.Replace(
                " ",
                ""
            );

        return resultado;
    }

    // ============================================================
    // PROCURA RECURSIVAMENTE AS PASTAS DE DESTINO
    // ============================================================

    void ProcurarDestinosRecursivamente(
        string pastaAtual,
        string codigo
    )
    {
        if (string.IsNullOrEmpty(pastaAtual))
            return;

        if (string.IsNullOrEmpty(codigo))
            return;

        string[] pastas =
            ModFileUtilsForDisplay.GetDirectories(
                pastaAtual
            );

        if (pastas == null)
            return;

        for (int i = 0; i < pastas.Length; i++)
        {
            string pasta =
                pastas[i];

            if (string.IsNullOrEmpty(pasta))
                continue;

            string nomePasta =
                ModFileUtilsForDisplay.GetFileName(
                    pasta
                );

            if (!string.IsNullOrEmpty(nomePasta))
            {
                string nomePastaNormalizado =
                    NormalizarCodigo(
                        nomePasta
                    );

                if (
                    nomePastaNormalizado.Contains(
                        codigo
                    )
                )
                {
                    destinosEncontrados.Add(
                        pasta
                    );

                    Debug.Log(
                        "Ciano Mobitec: destino encontrado: " +
                        nomePasta
                    );
                }
            }

            ProcurarDestinosRecursivamente(
                pasta,
                codigo
            );
        }
    }

    // ============================================================
    // PROCURA TODOS OS DESTINOS PARA UM CÓDIGO
    // ============================================================

    void ProcurarDestinos(
        string codigo
    )
    {
        destinosEncontrados.Clear();

        indiceDestinoAtual = -1;

        string pastaDest =
            CianoUtils.GetDestinationDisplaysDirectoryPath();

        if (string.IsNullOrEmpty(pastaDest))
        {
            Debug.Log(
                "Ciano Mobitec: não foi possível obter a pasta dest/."
            );

            return;
        }

        string codigoNormalizado =
            NormalizarCodigo(
                codigo
            );

        Debug.Log(
            "Ciano Mobitec: procurando código " +
            codigo +
            " normalizado como " +
            codigoNormalizado +
            " em:"
        );

        Debug.Log(
            pastaDest
        );

        ProcurarDestinosRecursivamente(
            pastaDest,
            codigoNormalizado
        );

        Debug.Log(
            "Ciano Mobitec: pesquisa terminou. " +
            destinosEncontrados.Count +
            " destino(s) encontrado(s)."
        );
    }

    // ============================================================
    // SELECIONA A PRÓXIMA OCORRÊNCIA ENCONTRADA
    // ============================================================

    void SelecionarProximoDestino(
        string codigo
    )
    {
        string codigoNormalizado =
            NormalizarCodigo(
                codigo
            );

        if (
            ultimoCodigoPesquisado !=
            codigoNormalizado
        )
        {
            ProcurarDestinos(
                codigoNormalizado
            );

            ultimoCodigoPesquisado =
                codigoNormalizado;
        }

        if (
            destinosEncontrados.Count == 0
        )
        {
            Debug.Log(
                "Ciano Mobitec: nenhum destino encontrado para o código " +
                codigo +
                "."
            );

            return;
        }

        indiceDestinoAtual++;

        if (
            indiceDestinoAtual
            >=
            destinosEncontrados.Count
        )
        {
            indiceDestinoAtual = 0;
        }

        string destino =
            destinosEncontrados[
                indiceDestinoAtual
            ];

        string nomeDestino =
            ModFileUtilsForDisplay.GetFileName(
                destino
            );

        Debug.Log(
            "Ciano Mobitec: destino selecionado " +
            (indiceDestinoAtual + 1) +
            "/" +
            destinosEncontrados.Count +
            ": " +
            nomeDestino
        );

        Debug.Log(
            "Ciano Mobitec: caminho do destino:"
        );

        Debug.Log(
            destino
        );
    }

    // ============================================================
    // CONFIRMAR CÓDIGO / ENTER
    // ============================================================

    void ConfirmarCodigo()
    {
        if (codigoDigitado == "")
        {
            Debug.Log(
                "Ciano Mobitec: nenhum código para confirmar."
            );

            return;
        }

        Debug.Log(
            "Ciano Mobitec: ENTER pressionado."
        );

        Debug.Log(
            "Ciano Mobitec: código confirmado = " +
            codigoDigitado
        );

        SelecionarProximoDestino(
            codigoDigitado
        );

        if (
            destinosEncontrados.Count > 0
            &&
            indiceDestinoAtual >= 0
            &&
            indiceDestinoAtual < destinosEncontrados.Count
        )
        {
            pastaDestinoAtual =
                destinosEncontrados[
                    indiceDestinoAtual
                ];

            texturasDestinoCarregadas.Clear();

            Debug.Log(
                "Ciano Mobitec: pasta de texturas atualizada:"
            );

            Debug.Log(
                pastaDestinoAtual
            );

            AtualizarTexturasDosLetreiros();

            IniciarProgramacaoDisplayFront();
        }

        LimparDisplayNumerico();

        if (mobitecDisplay != null)
        {
            mobitecDisplay.SetActive(true);

            Debug.Log(
                "Ciano Mobitec: display nativo ativado."
            );
        }
    }

    // ============================================================
    // ATUALIZA AS TEXTURAS DOS LETREIROS
    // ============================================================

    void AtualizarTexturasDosLetreiros()
    {
        if (
            string.IsNullOrEmpty(
                pastaDestinoAtual
            )
        )
        {
            return;
        }

        if (raizMeshes == null)
        {
            return;
        }

        Transform[] objetos =
            raizMeshes.GetComponentsInChildren<Transform>(true);

        for (
            int i = 0;
            i < objetos.Length;
            i++
        )
        {
            Transform objeto =
                objetos[i];

            if (objeto == null)
                continue;

            string tagLetreiro =
                ObterTagLetreiro(
                    objeto.name
                );

            if (tagLetreiro == null)
                continue;

            Renderer[] renderers =
                objeto.GetComponentsInChildren<Renderer>(true);

            if (
                renderers == null
                ||
                renderers.Length == 0
            )
            {
                continue;
            }

            for (
                int r = 0;
                r < renderers.Length;
                r++
            )
            {
                Renderer renderer =
                    renderers[r];

                if (renderer == null)
                    continue;

                Material[] materiais =
                    renderer.materials;

                if (
                    materiais == null
                    ||
                    materiais.Length == 0
                )
                {
                    continue;
                }

                bool materiaisAlterados = false;

                for (
                    int m = 0;
                    m < materiais.Length;
                    m++
                )
                {
                    Material material =
                        materiais[m];

                    if (material == null)
                        continue;

                    Texture texturaAtual =
                        material.mainTexture;

                    if (texturaAtual == null)
                        continue;

                    string nomeTextura =
                        NormalizarNomeTextura(
                            texturaAtual.name
                        );

                    if (
                        string.IsNullOrEmpty(
                            nomeTextura
                        )
                    )
                    {
                        continue;
                    }

                    Texture2D texturaDestino =
                        ObterTexturaDestino(
                            nomeTextura
                        );

                    if (texturaDestino == null)
                    {
                        continue;
                    }

                    if (
                        material.mainTexture
                        ==
                        texturaDestino
                    )
                    {
                        continue;
                    }

                    material.mainTexture =
                        texturaDestino;

                    materiaisAlterados = true;

                    Debug.Log(
                        "Ciano Mobitec: " +
                        tagLetreiro +
                        " -> " +
                        nomeTextura
                    );
                }

                if (materiaisAlterados)
                {
                    renderer.materials =
                        materiais;
                }
            }
        }
    }

    // ============================================================
    // OBTÉM TEXTURA DA LINHA
    // ============================================================

    Texture2D ObterTexturaDestino(
        string nomeArquivo
    )
    {
        if (
            string.IsNullOrEmpty(
                nomeArquivo
            )
        )
        {
            return null;
        }

        if (
            texturasDestinoCarregadas.ContainsKey(
                nomeArquivo
            )
        )
        {
            return texturasDestinoCarregadas[
                nomeArquivo
            ];
        }

        string caminho =
            EncontrarArquivoPorNome(
                pastaDestinoAtual,
                nomeArquivo
            );

        if (caminho == null)
        {
            return null;
        }

        Texture2D textura =
            CianoUtils.LoadTexture(
                caminho
            );

        if (textura == null)
        {
            Debug.Log(
                "Ciano Mobitec: não conseguiu carregar " +
                caminho
            );

            return null;
        }

        texturasDestinoCarregadas[
            nomeArquivo
        ] = textura;

        Debug.Log(
            "Ciano Mobitec: textura carregada no cache: " +
            nomeArquivo
        );

        return textura;
    }

    // ============================================================
    // IDENTIFICA O TIPO DO LETREIRO
    // ============================================================

    string ObterTagLetreiro(
        string nome
    )
    {
        if (string.IsNullOrEmpty(nome))
            return null;

        for (
            int i = 0;
            i < tagsLetreiros.Length;
            i++
        )
        {
            if (
                nome.IndexOf(
                    tagsLetreiros[i],
                    System.StringComparison.OrdinalIgnoreCase
                ) >= 0
            )
            {
                return tagsLetreiros[i];
            }
        }

        return null;
    }

    // ============================================================
    // NORMALIZA O NOME DA TEXTURA
    // ============================================================

    string NormalizarNomeTextura(
        string nome
    )
    {
        if (string.IsNullOrEmpty(nome))
            return null;

        string resultado =
            nome.Trim();

        if (
            resultado.EndsWith(
                ".png",
                System.StringComparison.OrdinalIgnoreCase
            )
            ||
            resultado.EndsWith(
                ".jpg",
                System.StringComparison.OrdinalIgnoreCase
            )
            ||
            resultado.EndsWith(
                ".jpeg",
                System.StringComparison.OrdinalIgnoreCase
            )
        )
        {
            return resultado;
        }

        return resultado + ".png";
    }

    // ============================================================
    // PROCURA ARQUIVO PELO NOME DENTRO DA PASTA DO DESTINO
    // ============================================================

    string EncontrarArquivoPorNome(
        string pasta,
        string nomeArquivo
    )
    {
        if (
            string.IsNullOrEmpty(pasta)
            ||
            string.IsNullOrEmpty(nomeArquivo)
        )
        {
            return null;
        }

        string[] arquivos;

        try
        {
            arquivos =
                ModFileUtilsForDisplay.GetFiles(
                    pasta
                );
        }
        catch (System.Exception erro)
        {
            Debug.Log(
                "Ciano Mobitec: erro ao procurar arquivos:"
            );

            Debug.Log(
                erro.ToString()
            );

            return null;
        }

        if (arquivos == null)
            return null;

        for (
            int i = 0;
            i < arquivos.Length;
            i++
        )
        {
            string nome =
                ModFileUtilsForDisplay.GetFileName(
                    arquivos[i]
                );

            if (
                string.Equals(
                    nome,
                    nomeArquivo,
                    System.StringComparison.OrdinalIgnoreCase
                )
            )
            {
                return arquivos[i];
            }
        }

        return null;
    }

    // ============================================================
    // CANCELAR / X
    // ============================================================

    void CancelarCodigo()
    {
        Debug.Log(
            "Ciano Mobitec: CANCEL pressionado."
        );

        codigoDigitado = "";

        LimparDisplayNumerico();

        if (mobitecDisplay != null)
        {
            mobitecDisplay.SetActive(false);

            Debug.Log(
                "Ciano Mobitec: display nativo escondido."
            );
        }

        destinosEncontrados.Clear();

        ultimoCodigoPesquisado = "";

        indiceDestinoAtual = -1;

        pastaDestinoAtual = "";

        texturasDestinoCarregadas.Clear();

        Debug.Log(
            "Ciano Mobitec: código limpo."
        );
    }

    // ============================================================
    //
    // EFEITO DE PROGRAMAÇÃO DO DISPLAY FRONT
    //
    // ============================================================

    bool displayFrontProgramando = false;

    float tempoProgramacaoDisplayFront = 3f;

    // ============================================================
    // INICIA A PROGRAMAÇÃO DO DISPLAY FRONT
    // ============================================================

    void IniciarProgramacaoDisplayFront()
    {
        if (displayFrontProgramando)
        {
            Debug.Log(
                "Ciano Mobitec: display front já está sendo programado."
            );

            return;
        }

        StartCoroutine(
            ProgramarDisplayFront()
        );
    }

    // ============================================================
    // PROGRAMA O DISPLAY FRONT
    // ============================================================

    System.Collections.IEnumerator ProgramarDisplayFront()
    {
        displayFrontProgramando = true;

        GameObject displayFront =
            null;

        if (raizMeshes != null)
        {
            Transform[] objetos =
                raizMeshes.GetComponentsInChildren<Transform>(true);

            for (
                int i = 0;
                i < objetos.Length;
                i++
            )
            {
                if (objetos[i] == null)
                    continue;

                if (
                    objetos[i].name.IndexOf(
                        "_display_front_",
                        System.StringComparison.OrdinalIgnoreCase
                    ) >= 0
                )
                {
                    displayFront =
                        objetos[i].gameObject;

                    break;
                }
            }
        }

        if (displayFront == null)
        {
            Debug.Log(
                "Ciano Mobitec: _display_front_ não encontrado para programação."
            );

            displayFrontProgramando = false;

            yield break;
        }

        displayFront.SetActive(false);

        Debug.Log(
            "Ciano Mobitec: _display_front_ apagado para programação."
        );

        yield return new WaitForSeconds(
            tempoProgramacaoDisplayFront
        );

        displayFront.SetActive(true);

        Debug.Log(
            "Ciano Mobitec: _display_front_ programado e reativado."
        );

        displayFrontProgramando = false;
    }
}