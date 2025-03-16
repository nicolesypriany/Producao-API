import Header from "@/modules/core/components/header";
import { useEffect } from "react";
import { FaRegEdit, FaRegTrashAlt } from "react-icons/fa";
import { useFormRawMaterial } from "../../hooks/useFormRawMaterial";
import { useGetRawMaterial } from "../../hooks/useGetRawMaterial";

const TableRawMaterial = () => {
  const { rawMaterials, handleGetRawMaterials } = useGetRawMaterial();
  const {
    editingRawMaterialId,
    handleCancelEditRawMaterial,
    handleEditRawMaterial,
    handleRemoveRawMaterial,
    handleSaveEditRawMaterial,
    handleSubmit,
    register,
  } = useFormRawMaterial();

  useEffect(() => {
    handleGetRawMaterials();
  }, []);

  // TODO - Adicionar ordenação nas colunas
  return (
    <h1>Materials</h1>
    // <Flex direction="column">
    //   <Header>Listagem de matéria-prima</Header>
    //   <form>
    //     {rawMaterials && rawMaterials.length > 0 && (
    //       <Table.Root w="100%">
    //         <Table.Header>
    //           <Table.Row>
    //             <Table.ColumnHeader>Id</Table.ColumnHeader>
    //             <Table.ColumnHeader>Nome</Table.ColumnHeader>
    //             <Table.ColumnHeader>Fornecedor</Table.ColumnHeader>
    //             <Table.ColumnHeader>Unidade</Table.ColumnHeader>
    //             <Table.ColumnHeader>Preço</Table.ColumnHeader>
    //             <Table.ColumnHeader></Table.ColumnHeader>
    //           </Table.Row>
    //         </Table.Header>
    //         <Table.Body>
    //           {rawMaterials.map((rawMaterial) => (
    //             <Table.Row key={rawMaterial.id}>
    //               <Table.Cell>{rawMaterial.id}</Table.Cell>
    //               {editingRawMaterialId === rawMaterial.id ? (
    //                 <>
    //                   <Table.Cell>
    //                     <Input
    //                       placeholder="Digite o nome da matéria-prima"
    //                       {...register("rawMaterialName", {
    //                         required: "Nome da matéria-prima é obrigatório",
    //                       })}
    //                     />
    //                   </Table.Cell>
    //                   <Table.Cell>
    //                     <Input
    //                       placeholder="Digite o fornecedor da matéria-prima"
    //                       {...register("rawMaterialProvider", {
    //                         required:
    //                           "Fornecedor da matéria-prima é obrigatório",
    //                       })}
    //                     />
    //                   </Table.Cell>
    //                   <Table.Cell>
    //                     <Input
    //                       placeholder="Digite a unidade da matéria-prima"
    //                       {...register("rawMaterialUnit", {
    //                         required: "Unidade da matéria-prima é obrigatória",
    //                       })}
    //                     />
    //                   </Table.Cell>
    //                   <Table.Cell>
    //                     <Input
    //                       placeholder="Digite o preço da matéria-prima"
    //                       {...register("rawMaterialPrice", {
    //                         required: "Preço da matéria-prima é obrigatório",
    //                       })}
    //                     />
    //                   </Table.Cell>
    //                 </>
    //               ) : (
    //                 <>
    //                   <Table.Cell>{rawMaterial.nome}</Table.Cell>
    //                   <Table.Cell>{rawMaterial.fornecedor}</Table.Cell>
    //                   <Table.Cell>{rawMaterial.unidade}</Table.Cell>
    //                   <Table.Cell>R${rawMaterial.preco}</Table.Cell>
    //                 </>
    //               )}

    //               <Table.Cell textAlign="end">
    //                 {/* TODO - criar um componente para mudar o botão */}
    //                 {editingRawMaterialId === rawMaterial.id ? (
    //                   <Button
    //                     mr={2}
    //                     bg="gray.500"
    //                     color="#fefefe"
    //                     borderWidth={1}
    //                     borderColor={{ base: "black", _dark: "white" }}
    //                     onClick={handleSubmit((data) =>
    //                       handleSaveEditRawMaterial(rawMaterial.id, data)
    //                     )}
    //                   >
    //                     Salvar
    //                   </Button>
    //                 ) : (
    //                   <IconButton
    //                     bg="transparent"
    //                     color={{ base: "black", _dark: "#fefefe" }}
    //                     onClick={() => handleEditRawMaterial(rawMaterial.id)}
    //                   >
    //                     <FaRegEdit />
    //                   </IconButton>
    //                 )}
    //                 {editingRawMaterialId === rawMaterial.id ? (
    //                   <Button
    //                     bg="red.500"
    //                     color="#fefefe"
    //                     onClick={() => handleCancelEditRawMaterial()}
    //                   >
    //                     Cancelar
    //                   </Button>
    //                 ) : (
    //                   <IconButton
    //                     bg="transparent"
    //                     color={{ base: "black", _dark: "#fefefe" }}
    //                     onClick={() =>
    //                       handleRemoveRawMaterial(
    //                         rawMaterial.id,
    //                         rawMaterial.nome
    //                       )
    //                     }
    //                   >
    //                     <FaRegTrashAlt />
    //                   </IconButton>
    //                 )}
    //               </Table.Cell>
    //             </Table.Row>
    //           ))}
    //         </Table.Body>
    //       </Table.Root>
    //     )}
    //   </form>
    // </Flex>
  );
};

export default TableRawMaterial;
