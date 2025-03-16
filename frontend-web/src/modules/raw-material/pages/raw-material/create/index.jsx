import Footer from "@/modules/core/components/footer";
import Header from "@/modules/core/components/header";
import { useCreateRawMaterial } from "@/modules/raw-material/hooks/useCreateRawMaterial";

const RawMaterialPage = () => {
  const { errors, handleSubmit, onSubmit, register } = useCreateRawMaterial();

  return (
    <h1>Raw material</h1>
    // <form onSubmit={handleSubmit(onSubmit)}>
    //   <Flex width="100%" justifyContent="center">
    //     <Flex p={4} direction="column" w="50%">
    //       <Header>Cadastrar matéria-prima</Header>

    //       <Flex direction="column" gap={4}>
    //         <Field
    //           label="Nome"
    //           invalid={!!errors.name}
    //           errorText={errors.name?.message}
    //         >
    //           <Input
    //             type="text"
    //             placeholder="Digite o nome da matéria-prima"
    //             {...register("name", {
    //               required: "Nome da matéria-prima é obrigatório",
    //             })}
    //           />
    //         </Field>

    //         <Field
    //           label="Fornecedor"
    //           invalid={!!errors.supplier}
    //           errorText={errors.supplier?.message}
    //         >
    //           <Input
    //             type="text"
    //             placeholder="Digite o fornecedor da matéria-prima"
    //             {...register("supplier", {
    //               required: "Nome do fornecedor é obrigatório",
    //             })}
    //           />
    //         </Field>

    //         <Field
    //           label="Unidade"
    //           invalid={!!errors.unit}
    //           errorText={errors.unit?.message}
    //         >
    //           <Input
    //             type="text"
    //             placeholder="Digite a unidade da matéria-prima"
    //             {...register("unit", {
    //               required: "Nome da unidade é obrigatório",
    //             })}
    //             maxLength={5}
    //           />
    //         </Field>

    //         <Field
    //           label="Preço"
    //           invalid={!!errors.price}
    //           errorText={errors.price?.message}
    //         >
    //           <Input
    //             placeholder="R$0.00"
    //             {...register("price", {
    //               required: "Quantidade é obrigatório",
    //             })}
    //           />
    //         </Field>

    //         <Footer />
    //       </Flex>
    //     </Flex>
    //   </Flex>
    // </form>
  );
};

export default RawMaterialPage;
