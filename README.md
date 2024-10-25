**Talk Libras** é um projeto desenvolvido para facilitar a comunicação entre pessoas surdas e ouvintes, promovendo a inclusão através da Língua Brasileira de Sinais (Libras). O projeto não tem fins lucrativos e está disponível de forma aberta, incentivando a contribuição e uso de maneira colaborativa.

Este projeto utiliza recursos de código aberto, como o **VLibras**, uma ferramenta mantida pelo governo brasileiro sob a licença GPL, que faz a tradução de conteúdos digitais para Libras. Mais detalhes sobre o VLibras estão disponíveis aqui: _https://www.gov.br/governodigital/pt-br/acessibilidade-e-usuario/vlibras_

**Funcionalidades**

A interface principal do Talk Libras oferece funcionalidades robustas, como:

- **Transcrição de voz em texto em tempo real**: Utilizando **WebSocket** e o serviço de transcrição de voz **Deepgram**, que emprega **Inteligência Artificial (IA)** para garantir alta precisão na conversão de áudio em texto.
- **Tradução de texto para Libras**: O texto transcrito ou inserido manualmente é imediatamente traduzido para Libras, exibido por um avatar 3D.
- **Avatar 3D interativo**: O avatar representa de forma visual as expressões em Libras, tornando a comunicação acessível e compreensível para usuários surdos.
Esses recursos facilitam a comunicação em diversos contextos, como ambientes educacionais, atendimento ao cliente ou até mesmo conversas cotidianas.

**Exemplo de Uso**
![image](https://github.com/user-attachments/assets/638fe52a-d51f-4e47-9033-217d6688b98c)


Na imagem acima, vemos o funcionamento da interface. O usuário pode ativar o microfone para que sua fala seja transcrita para texto e, em seguida, traduzida para Libras, com o auxílio do avatar 3D à direita. A integração com Deepgram, utilizando IA, garante uma transcrição de voz precisa e rápida, tornando o processo eficiente.

**Tecnologias Utilizadas**

- **VLibras**: Plataforma para tradução de conteúdo digital para Libras.
- **WebSocket**: Utilizado para comunicação em tempo real, garantindo a rápida transmissão de dados de áudio para texto.
- **Deepgram**: API especializada em transcrição de voz em texto com suporte de Inteligência Artificial, permitindo que o áudio seja processado em tempo real.
- **C# e Unity**: Linguagem e motor gráfico usados para o desenvolvimento da interface e do avatar 3D.
- **API do governo**: Integração com a API que dá acesso ao dicionário de Libras.

**Perspectivas Futuras**

Nas próximas atualizações, o projeto Talk Libras incluirá:

- **Correções de bugs** para aprimorar a estabilidade da aplicação.
- **Melhorias gerais** na interface e usabilidade.
- **Transcrição para diferentes idiomas**, permitindo que o usuário fale em outras línguas e o sistema traduza para Libras ou para outro idioma de destino.
