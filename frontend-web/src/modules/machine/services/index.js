const api = import.meta.env.VITE_BACKEND_URL;

export const getMachine = async () => {
  try {
    const response = await fetch(`${api}/Maquina`);

    if (!response.ok) {
      throw new Error("Erro ao buscar máquina");
    }

    return response.json();
  } catch (error) {
    console.error(error);
  }
};

export const createMachine = async (data) => {
  try {
    const response = await fetch(`${api}/Maquina`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ nome: data.machineName, marca: data.brand }),
    });

    if (!response.ok) {
      throw new Error("Erro ao cadastrar máquina");
    }

    alert("Máquina cadastrada com sucesso!");
  } catch (error) {
    console.error("Error:", error);
  }
};

export const editMachine = async (id, { nome, marca }) => {
  try {
    const response = await fetch(`${api}/Maquina/${id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ nome, marca }),
    });

    if (!response.ok) {
      throw new Error("Erro ao editar máquina");
    }

    alert("Máquina editada com sucesso!");
  } catch (error) {
    console.error(error);
  }
};

export const removeMachine = async (id) => {
  try {
    const response = await fetch(`${api}/Maquina2/${id}`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
      },
    });

    // TODO - remover exception e colocar em um nível acima
    if (!response.ok) {
      throw new Error("Erro ao remover máquina");
    }
  } catch (error) {
    console.error(error);
  }
};
