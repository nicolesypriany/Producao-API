import { useState } from "react";
import { useForm } from "react-hook-form";
import {
  editRawMaterial as editRawMaterialService,
  removeRawMaterial as removeRawMaterialService,
} from "../services";

export function useFormRawMaterial() {
  const [rawMaterials, setRawMaterials] = useState([]);
  const [editingRawMaterialId, setEditingRawMaterialId] = useState(null);

  const { register, handleSubmit, reset } = useForm();

  const handleEditRawMaterial = async (idRawMaterial) =>
    setEditingRawMaterialId(idRawMaterial);
  const handleCancelEditRawMaterial = async () => {
    setEditingRawMaterialId(null);
    reset();
  };

  const handleSaveEditRawMaterial = async (idRawMaterial, data) => {
    try {
      await editRawMaterialService(idRawMaterial, {
        nome: data.rawMaterialName,
        marca: data.markName,
      });
      setRawMaterials((prevRawMaterials) =>
        prevRawMaterials.map((rawMaterial) =>
          rawMaterial.id === idRawMaterial
            ? {
                ...rawMaterial,
                nome: data.rawMaterialName,
                marca: data.markName,
              }
            : rawMaterial
        )
      );
      setEditingRawMaterialId(null);
    } catch (error) {
      console.error("Error:", error);
    }
  };

  const handleRemoveRawMaterial = async (idRawMaterial) => {
    await removeRawMaterialService(idRawMaterial);
    setRawMaterials((prevRawMaterials) =>
      prevRawMaterials.filter((rawMaterial) => rawMaterial.id !== idRawMaterial)
    );
  };

  return {
    editingRawMaterialId,
    register,
    handleSubmit,
    handleEditRawMaterial,
    handleRemoveRawMaterial,
    handleSaveEditRawMaterial,
    handleCancelEditRawMaterial,
  };
}
