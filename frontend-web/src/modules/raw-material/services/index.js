const api = import.meta.env.VITE_BACKEND_URL;

export const getRawMaterial = async () => {
  try {
    const response = await fetch(`${api}/MateriaPrima`);

    if (!response.ok) {
      throw new Error("Erro ao buscar matéria-prima");
    }

    return response.json();
  } catch (error) {
    console.error(error);
  }
};

export const createRawMaterial = async (data) => {
  try {
    const response = await fetch(`${api}/MateriaPrima`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        nome: data.name,
        fornecedor: data.supplier,
        unidade: data.unit,
        preco: data.price,
      }),
    });

    if (!response.ok) {
      throw new Error("Erro ao cadastrar matéria-prima");
    }

    alert("Matéria-prima cadastrada com sucesso!");
  } catch (error) {
    console.error(error);
  }
};

export const editRawMaterial = async (id, data) => {
  try {
    console.log("Edditing", id, data);
  } catch (error) {
    console.error(error);
  }
};

export const removeRawMaterial = async (id) => {
  try {
    console.log("Removing", id);
  } catch (error) {
    console.error(error);
  }
};
