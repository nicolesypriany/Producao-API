import { useState } from "react";
import { getRawMaterial as getRawMaterialService } from "../services";

export function useGetRawMaterial() {
  const [rawMaterials, setRawMaterials] = useState([]);

  const handleGetRawMaterials = async () => {
    const rawMaterials = await getRawMaterialService();
    setRawMaterials(rawMaterials);
  };

  return {
    rawMaterials,
    handleGetRawMaterials,
  };
}
